using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.GraphModels;
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

	    protected internal List<State> GetInitialStates(IList<State> initialStatesInSystem, IList<State> allStates)
	    {
		    if (InitialStateCondition == null)
			    return initialStatesInSystem.ToList();
		    else
			    return allStates.Where(state => InitialStateCondition.CheckForState(state.FluentValues))
					    .ToList();
	    }

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
				newStatesForThatState =
					GoFurtherFromThatState(world.Connections[Program[step].ActionId][state], notEngagedAgents,
						state, step, ref executableAlways);

				// get released
				BitSet releasedFluents;
				if (world.ReleasedFluents.ContainsKey(Program[step].ActionId))
				{
					releasedFluents =
						GetReleasedFluents(world.ReleasedFluents[Program[step].ActionId][state], world,
							state,
							step, notEngagedAgents);
				}
				else releasedFluents = new BitSet(0); // zero releases causes in that world
				newStates.AddRange(minimiser.MinimaliseChanges(state, newStatesForThatState, releasedFluents.Set, world.NonInertialFluents.Set));
			}

		    return newStates;
	    }




	    private List<State> GoFurtherFromThatState(IList<AgentSetChecker> setCheckers, int notEngagedAgents, State state, 
			int step, ref bool executableAlways)
	    {
		    int howManyStateAvailable = 0;
			Dictionary<State, int> intersectedStatesSet = new Dictionary<State, int>();

		    if (setCheckers.Count == 0) // empty action
		    {
				return  new List<State>() {state};
		    }

		    foreach (AgentSetChecker setChecker in setCheckers)
		    {
#if DEBUG
			    Logger.Log("checking for " + setChecker.AgentsSet);
			    Logger.Log("can be executed = "
							+ setChecker.CanBeExecutedByAgentsSet(Program[step].AgentsSet.AgentSet));
			    Logger.Log("eng = " + !setChecker.UsesAgentFromSet(notEngagedAgents));
#endif

			    if (ActionCanBeExecutedByThoseAgents(setChecker, Program[step].AgentsSet.AgentSet,
				    notEngagedAgents)) //
			    {
#if DEBUG
				    Logger.Log("pass, states here = " + string.Join(", ", setChecker.Edges));
#endif
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
#if DEBUG
			Logger.Log("intersecting here = "
						+ string.Join(", ", intersectedStatesSet.Select(p => "(" + p.Value + ", " + p.Key + ")")));
#endif
		    return GetAvailableStatesFromIntersected(intersectedStatesSet, howManyStateAvailable);
	    }

	    private BitSet GetReleasedFluents(IList<ReleasesWithAgentsSet> releases, World world, State state, int step, int notEngagedAgents)
	    {
			if (releases.Count == 0) // empty action
			{
				return new BitSet(0);
			}

		    int set = 0;
			BitSetOperator bop = new BitSetOperator();
			foreach (ReleasesWithAgentsSet release in releases)
			{
				if (ActionCanBeExecutedByThoseAgents(release, Program[step].AgentsSet.AgentSet,
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
		

		public enum QueryType
		{
			Executable,
			After,
			Engaged
		}

	}
}
