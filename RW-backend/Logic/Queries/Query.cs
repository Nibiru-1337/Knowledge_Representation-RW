using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;
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

	    protected internal List<State> GetInitialStates(IList<State> allInitialStates)
	    {
			List<State> states = new List<State>(allInitialStates.Count);
			states.AddRange(allInitialStates.Where(state => InitialStateCondition.CheckForState(state.FluentValues)));
			return states;
	    }

		protected internal ProgramExecutionResult ExecuteProgram(World world, MinimiserOfChanges minimiser, 
			List<State> initialStates, int notEngagedAgents = 0)
	    {
			// tylko patrzymy na program, który juz tutaj jest zapisany
			// TODO: pewnie coś załatwić, żeby nie używać tylu list

		    List<State> states = initialStates;

		    bool executableAlways = true;
			List<State> newStates = new List<State>();
			List<State> newStatesForThatState = new List<State>();
			Dictionary<State, int> intersectedStatesSet = new Dictionary<State, int>();
			List<KeyValuePair<int, State>>[] programExecution = new List<KeyValuePair<int, State>>[Program.Count];


			var result = new ProgramExecutionResult();
			for (int i = 0; i < Program.Count; i++)
		    {
			    // wykonujemy program
			    newStates.Clear();
				Logger.Log("~~~~");
				Logger.Log("states available for i = " + i);
				Logger.Log("=> " + string.Join(", ", states));

				if (!world.Connections.ContainsKey(Program[i].ActionId))
				{
					//TODO: wrong actionID? exception?
					result.Executable = Executable.Never;
					return result;
				}

				programExecution[i] = new List<KeyValuePair<int, State>>(states.Count);



			    for (int index = 0; index < states.Count; index++)
			    {
				    var state = states[index];
				    newStatesForThatState.Clear();
				    intersectedStatesSet.Clear();
				    int howManyStateAvailable = 0;

				    Logger.Log("~* state = " + state);
				    if (world.Connections[Program[i].ActionId][state].Count == 0)
				    {
						executableAlways = false;
						intersectedStatesSet.Clear();
					}

				    foreach (AgentSetChecker setChecker in world.Connections[Program[i].ActionId][state])
				    {
					    Logger.Log("checking for " + setChecker.AgentsSet);
					    Logger.Log("can be executed = "
									+ setChecker.CanBeExecutedByAgentsSet(Program[i].AgentsSet.AgentSet));
					    Logger.Log("eng = " + !setChecker.UsesAgentFromSet(notEngagedAgents));

					    if (setChecker.CanBeExecutedByAgentsSet(Program[i].AgentsSet.AgentSet)
							&& !setChecker.UsesAgentFromSet(notEngagedAgents)) //
					    {
						    Logger.Log("pass, states here = " + string.Join(", ", setChecker.Edges));
						    howManyStateAvailable++;
						    if (setChecker.Edges.Count == 0)
						    {
							    executableAlways = false;
							    intersectedStatesSet.Clear();
							    break;
						    }
						    else
						    {
							    foreach (State edge in setChecker.Edges)
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
					    }
				    }

				    Logger.Log("intersecting here = "
								+ string.Join(", ",
									intersectedStatesSet.Select(p => "(" + p.Value + ", " + p.Key + ")")));
				    newStatesForThatState =
					    intersectedStatesSet.Where(pair => pair.Value == howManyStateAvailable)
						    .Select(pair => pair.Key)
						    .ToList();


				    newStates.AddRange(minimiser.MinimaliseChanges(state, newStatesForThatState));
			    }

			    if (newStates.Count == 0)
			    {
					Logger.Log("new states count = 0, so never executable");
					result.Executable = Executable.Never;
				    return result;
			    }
			    states = newStates.Distinct().ToList(); // żeby nie powtarzać

		    }

			Logger.Log("last states = " + string.Join(", ", states));
		    result.ReachableStates = states;
		    result.Executable = states.Count == 0
			    ? Executable.Never
			    : (executableAlways ? Executable.Always : Executable.Sometimes);
			Logger.Log("executable = " + result.Executable);
		    return result;

	    }

		public static Query Create(string queryString)
		{
			throw new NotImplementedException();
		}

		public enum QueryType
		{
			Executable,
			After,
			Engaged
		}

	}
}
