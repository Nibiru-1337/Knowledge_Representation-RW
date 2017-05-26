using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;

[assembly: InternalsVisibleTo("RW-tests")]
namespace RW_backend.Logic.Queries
{
    /// <summary>
    /// Reprezentacja kwerendy
    /// </summary>
    public abstract class Query
    {
	    public abstract QueryType Type { get; }
        public abstract QueryResult Evaluate(World world);


		public IReadOnlyList<ActionAgentsPair> Program { get; }
	    public bool Always { get; }
		public LogicClause InitialStateCondition { get; } // alfa


		protected Query(IReadOnlyList<ActionAgentsPair> program, LogicClause initialStateCondition, bool always)
		{
			Program = program;
			Always = always;
			InitialStateCondition = initialStateCondition;
		}

		public ProgramExecutionResult GetDetailsFromExecution(World world)
		{
			MinimiserOfChanges minimiser = new MinimiserOfChanges();
			var initial = GetInitialStates(world.InitialStates, world.States);
			return ExecuteProgram(world, minimiser, initial);
		}

		protected internal List<State> GetInitialStates(IList<State> initialStatesInSystem, IList<State> allStates)
	    {
		    if (InitialStateCondition == null)
			    return initialStatesInSystem.ToList();
		    else
			    return allStates.Where(state => InitialStateCondition.CheckForState(state.FluentValues))
					    .ToList();
	    }

		// top (0) level of execution
		protected internal ProgramExecutionResult ExecuteProgram(World world, MinimiserOfChanges minimiser, 
			List<State> initialStates, int notEngagedAgents = 0)
	    {
		    List<State> states = initialStates;
		    bool executableAlways = true;
			//List<KeyValuePair<int, State>>[] programExecution = new List<KeyValuePair<int, State>>[Program.Count];


			var result = new ProgramExecutionResult();
			for (int i = 0; i < Program.Count; i++)
		    {
			    // wykonujemy program
#if DEBUG
                Logger.Log("~~~~");
				Logger.Log("states available for i = " + i);
				Logger.Log("=> " + string.Join(", ", states));
#endif

				if (!world.Connections.ContainsKey(Program[i].ActionId))
				{
					//TODO: wrong actionID? exception?
					result.Executable = Executable.Never;
					throw new ArgumentException("wrong action id");
					return result;
				}

				//programExecution[i] = new List<KeyValuePair<int, State>>(states.Count);


			    var newStates = TakeNextTimeStep(i, states, world, notEngagedAgents, minimiser,
				    ref executableAlways);
			    

			    if (newStates.Count == 0)
			    {
#if DEBUG
					Logger.Log("new states count = 0, so never executable");
#endif
					result.Executable = Executable.Never;
				    return result;
			    }

			    states = newStates.Distinct().ToList(); // żeby nie powtarzać

		    }

#if DEBUG
			Logger.Log("last states = " + string.Join(", ", states));
#endif
		    result.ReachableStates = states;
		    result.Executable = states.Count == 0
			    ? Executable.Never
			    : (executableAlways ? Executable.Always : Executable.Sometimes);
#if DEBUG
			Logger.Log("executable = " + result.Executable);
#endif
		    return result;

	    }
		

		// inner (1) level of execution
	    List<State> TakeNextTimeStep(int step, List<State> states, World world, int notEngagedAgents, 
			MinimiserOfChanges minimiser, ref bool executableAlways)
	    {
			List<State> newStatesForThatState = new List<State>();
			List<State> newStates = new List<State>();
			
			for (int index = 0; index < states.Count; index++)
			{
				var state = states[index];
#if DEBUG
				Logger.Log("~* state = " + state);
#endif
				bool emptyAction = false;
				newStatesForThatState =
					GoFurtherFromThatState(world.Connections[Program[step].ActionId][state], notEngagedAgents,
						state, step, ref executableAlways, out emptyAction);

				// get released
				BitSet releasedFluents;
				if (world.ReleasedFluents.ContainsKey(Program[step].ActionId))
				{
					releasedFluents =
						GetReleasedFluents(world.ReleasedFluents[Program[step].ActionId][state], world,
							state,
							step, notEngagedAgents);
				}
				else releasedFluents = new BitSet(0); // zero releases clauses in that world
				UpdateNewStates(newStates, emptyAction, releasedFluents, state, world, minimiser, newStatesForThatState);
			}

		    return newStates;
	    }

		// inner (2) level od execution
	    private List<State> GoFurtherFromThatState(IList<AgentSetChecker> setCheckers, int notEngagedAgents, State state, 
			int step, ref bool executableAlways, out bool emptyAction)
	    {
		    int howManyStateAvailable = 0;
			Dictionary<State, int> intersectedStatesSet = new Dictionary<State, int>();

			emptyAction = true; // jeśli będzie jakiś setChecker, to zmienimy
			if (setCheckers.Count == 0) // empty action
		    {
				return  new List<State>() {state}; // chyba że releases
		    }
			
		    foreach (AgentSetChecker setChecker in setCheckers)
		    {
#if DEBUG
			    Logger.Log("checking for " + setChecker.AgentsSet);
			    Logger.Log("can be executed = "
							+ setChecker.CanBeExecutedByAgentsSet(Program[step].AgentsSet.AgentBitSet));
			    Logger.Log("eng = " + !setChecker.UsesAgentFromSet(notEngagedAgents));
#endif

			    if (ActionCanBeExecutedByThoseAgents(setChecker, Program[step].AgentsSet.AgentBitSet,
				    notEngagedAgents)) //
			    {
#if DEBUG
				    Logger.Log("pass, states here = " + string.Join(", ", setChecker.Edges));
#endif
				    emptyAction = false; // nie, ponieważ istnieje taki zbiór agentów, który prowadzi do czegoś, nawet jeśli do sprzeczności
				    howManyStateAvailable++;
				    if (setChecker.Edges.Count == 0) // impossible action in that state with those agents
				    {
					    executableAlways = false;
					    intersectedStatesSet.Clear();
					    break;
				    }
				    AddToIntersected(setChecker.Edges, intersectedStatesSet);
			    }
		    }
			if(emptyAction)
				return new List<State>() {state}; // żeby nie liczyć niepotrzebnie w sumie pustych rzeczy

#if DEBUG
			Logger.Log("intersecting here = "
						+ string.Join(", ", intersectedStatesSet.Select(p => "(" + p.Value + ", " + p.Key + ")")));
#endif
		    return GetAvailableStatesFromIntersected(intersectedStatesSet, howManyStateAvailable);
	    }

		#region helpers methods
		private void UpdateNewStates(List<State> newStates, bool emptyAction,
		    BitSet releasedFluents, State state, World world, MinimiserOfChanges minimiser, List<State> newStatesForThatState)
	    {
			if (emptyAction)
			{
				if (Equals(releasedFluents, BitSet.EmptySet)) // nic się nie zmienia
				{
					// dodajemy ten stan i już
					newStates.Add(state);
				}
				else
				{
					newStates.AddRange(GetReleasedStatesWhenEmptyAction(state, releasedFluents));
				}
			}
			else
				newStates.AddRange(minimiser.MinimaliseChanges(state, newStatesForThatState, releasedFluents.Set, 
					world.NonInertialFluents.Set));
		}

	    private List<State> GetReleasedStatesWhenEmptyAction(State state,
		    BitSet releasedFluents)
	    {
		    var released = releasedFluents.GetAllFromSet(); // na pewno distinct
			var newStates = new List<State>((int)Math.Pow(released.Count, 2));
			newStates.Add(state);
			BitSetFactory bsfactory = new BitSetFactory();
			foreach (var fluent in released)
		    {
				List<State> newSts = new List<State>(newStates.Count);
			    foreach (State st in newStates) // podwajamy stany
			    {
				    if (st.FluentValue(fluent))
					    newSts.Add(new State(bsfactory.CreateFromStateAndSetValue(0, fluent, st.FluentValues)));
				    else newSts.Add(new State(bsfactory.CreateFromStateAndSetValue(1, fluent, st.FluentValues)));
			    }
				newStates.AddRange(newSts);
		    }
		    return newStates;
	    } 

	    private BitSet GetReleasedFluents(IList<ReleasesWithAgentsSet> releases, World world, State state, int step, int notEngagedAgents)
	    {
			if (releases.Count == 0) // empty action in terms of releases
			{
				return new BitSet(0);
			}

		    int set = 0;
			BitSetOperator bop = new BitSetOperator();
			foreach (ReleasesWithAgentsSet release in releases)
			{
				if (ActionCanBeExecutedByThoseAgents(release, Program[step].AgentsSet.AgentBitSet,
					notEngagedAgents)) //
				{
					set = bop.GetSumOfSets(set, release.FluentsReleased.Set);
				}
			}

		    return new BitSet(set);

	    }

	    private List<State> GetAvailableStatesFromIntersected(Dictionary<State, int> intersectedStatesSet, int howManyStatesAvailable)
	    {
			return intersectedStatesSet.Where(pair => pair.Value == howManyStatesAvailable)
					.Select(pair => pair.Key)
					.ToList();
		}

	    private void AddToIntersected(List<State> edges, Dictionary<State, int> intersectedStatesSet)
	    {
			foreach (State edge in edges)
			{
				if (intersectedStatesSet.ContainsKey(edge))
				{
					intersectedStatesSet[edge]++;
				}
				else
				{
					intersectedStatesSet.Add(edge, 1);
				}
			}
		}

	    private bool ActionCanBeExecutedByThoseAgents(AgentSetContainer checker,
		    int agentsAvailable, int notEngagedAgents)
	    {
			return checker.CanBeExecutedByAgentsSet(agentsAvailable)
					&& !checker.UsesAgentFromSet(notEngagedAgents);
	    }

		#endregion
		public enum QueryType
		{
			Executable,
			After,
			Engaged,
			ExecutePath,
		}

	}
}
