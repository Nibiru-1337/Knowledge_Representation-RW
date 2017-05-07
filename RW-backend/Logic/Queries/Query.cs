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


		protected Query(IReadOnlyList<ActionAgentsPair> program, bool always, LogicClause initialStateCondition)
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

		protected internal ProgramExecutionResult ExecuteProgram(World world, List<State> initialStates, int notEngagedAgents = 0)
	    {
			// tylko patrzymy na program, który juz tutaj jest zapisany
			// TODO: pewnie coś załatwić, żeby nie używać tylu list

		    List<State> states = initialStates;

		    bool executableAlways = true;
			List<State> newStates = new List<State>();
			List<State> newStatesForThatState = new List<State>();
			Dictionary<State, int> intersectedStatesSet = new Dictionary<State, int>();


			var result = new ProgramExecutionResult();
			for (int i = 0; i < Program.Count; i++)
		    {
			    // wykonujemy program
			    newStates.Clear();

				if (!world.Connections.ContainsKey(Program[i].ActionId))
				{
					result.Executable = Executable.Never;
					return result;
				}


				foreach (var state in states)
			    {
					newStatesForThatState.Clear();
					intersectedStatesSet.Clear();
				    int howManyStateAvailable = 0;

				    
				    foreach (AgentSetChecker setChecker in world.Connections[Program[i].ActionId][state])
				    {
					    if (setChecker.CanBeExecutedByAgentsSet(Program[i].AgentsSet.AgentSet)
							&& !setChecker.UsesAgentFromSet(notEngagedAgents)) //
					    {
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

				    newStatesForThatState =
					    intersectedStatesSet.Where(pair => pair.Value == howManyStateAvailable)
						    .Select(pair => pair.Key)
						    .ToList();

					// TODO: minimalizacja zmian na newStatesForThatState
					// dopiero potem dodajemy do listy wszystkich stanów
					newStates.AddRange(newStatesForThatState);

			    }

			    if (newStates.Count == 0)
			    {
					result.Executable = Executable.Never;
				    return result;
			    }
			    states = newStates.Distinct().ToList(); // żeby nie powtarzać

		    }
		    result.ReachableStates = states;
		    result.Executable = states.Count == 0
			    ? Executable.Never
			    : (executableAlways ? Executable.Always : Executable.Sometimes);
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
