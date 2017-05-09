//#define EXTENDED_DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_backend.Models.World
{
    /// <summary>
    /// Reprezentuje modelowany świat za pomocą grafu stanów
    /// </summary>
    public class World
    {
	    public const int MaxFluentCount = 31; // TODO: czemu nie 32?

        // so far the constructor creates an array of all possible states
        // without any edges in between them (edges are added with AddCauses)
        public  IList<State> States { get; private set; }

        //TODO delete if ActionIds not needed in World class
        // information about the actions in this world
        public IList<int> ActionIds { get; private set; }

        //connections b/w states are represented with a chain of dictonaries
        // [int(ActionId)] [State(starting state)] -> List<AgentSetChecker>
        public Dictionary<int, Dictionary<State, IList<AgentSetChecker>>> Connections { get; private set; }
		public IList<State> InitialStates { get; private set; } 
		public BitSet NonInertialFluents { get; private set; }
		public Dictionary<KeyValuePair<int, AgentsSet>, BitSet> ReleasedFluents { get; private set; } 

        public World(int fluentsCount, IList<LogicClause> alwaysList, IList<LogicClause> initiallyList, 
			BitSet nonInertialFluents, Dictionary<KeyValuePair<int, AgentsSet>, BitSet> releasedFluents)
        {
	        NonInertialFluents = nonInertialFluents;
	        ReleasedFluents = releasedFluents;

	        if (fluentsCount > MaxFluentCount)
				throw new ArgumentException("max fluent count = " + MaxFluentCount);

            int totalNodes = (int)1<<fluentsCount;
            Connections = new Dictionary<int, Dictionary<State, IList<AgentSetChecker>>>();
            States = new List<State>(totalNodes);
            ActionIds = new List<int>();

            for (int i = 0; i < totalNodes; i++)
            {
                if (alwaysList != null && alwaysList.Count > 0)
                {
                    bool goodToGo = true;
                    foreach (var always in alwaysList)
                    {
                        if (!always.CheckForState(i))
                        {
                            goodToGo = false;
                            break;
                        }
                    }
                    if (goodToGo) States.Add(new State(i));
                }
                else
                {
                    States.Add(new State(i));
                }
            }
	        if (initiallyList != null)
	        {
		        InitialStates =
			        States.Where(
				        state =>
					        initiallyList.All(initially => initially.CheckForState(state.FluentValues)))
				        .ToList();
	        }
	        else InitialStates = States.ToList();
        }

        //add the edges concerning a particular causes action
        public void AddCauses(IList<Causes>causesList, int actionCount)
        {
            //for every unique ActionID
            for (int i = 0; i < actionCount; i++)
            {
				Logger.Log("<ACTION> " + i);

                var stateToAgentSetCheckers = new Dictionary<State, IList<AgentSetChecker>>();
                //for every starting state
                foreach (var startingState in States)
                {
#if DEBUG
					Logger.Log("state = " + startingState);
#endif

					//TODO check assumption that actionCount is numer of unique ActionIDs, and that they are always sequential
					List<Causes> sameID = causesList.Where(a => a.Action == i).ToList();
                    var ascList = new List<AgentSetChecker>(sameID.Count);
                    //for each action with the same ActionID
                    foreach (var causesClause in sameID)
                    {
#if DEBUG
						Logger.Log("causes nr " + causesClause);
#endif
                        //if there is no conditions or state satisfies conditions
                        if (causesClause.InitialCondition == null 
							|| causesClause.InitialCondition.CheckForState(startingState.FluentValues))
                        {
#if DEBUG
							Logger.Log("state satisfies condition");
#endif
                            //get all states that have proper result fluents as caused by action
                            List<State> possibleResults = new List<State>();

                            foreach (var endingState in States)
                            {
#if DEBUG && EXTENDED_DEBUG
                                Logger.Log("checking for " + endingState + ", logic clause = " + causesClause.Effect);
#endif
                                if (causesClause.Effect.CheckForState(endingState.FluentValues))
                                {
#if DEBUG && EXTENDED_DEBUG
									Logger.Log("(pass)");
#endif
                                    possibleResults.Add(endingState);
                                }
                                else
                                {
#if DEBUG && EXTENDED_DEBUG
	                                Logger.Log("(not)");
#endif
                                }
                            }

                            //find state(s) that have the least amount of changed fluents
							// TODO: think about Minimalisation in building world
							// especially about these statement:
							// SHOOT by Bob causes !alive if loaded
							// SHOOT by Bob causes !loaded
                            //List<State> result = new List<State>();
                            //int min = Int32.MaxValue;
                            //foreach (var endingState in possibleResults)
                            //{
                            //    int diff = _StateDifference(startingState, endingState);
                            //    if (min > diff)
                            //    {
                            //        min = diff;
                            //        result.Clear();
                            //        result.Add(endingState);
                            //    }
                            //    else if (min == diff)
                            //    {
                            //        result.Add(endingState);
                            //    }
                            //}
                            //add the results of action from starting state into ActionSetChecker
	                        var result = possibleResults;
                            ascList.Add(new AgentSetChecker(causesClause.AgentsSet.AgentSet, result));
                        }
                    }
                    stateToAgentSetCheckers.Add(startingState, ascList);
                }
                Connections.Add(i, stateToAgentSetCheckers);
                //TODO delete if ActionIds not needed in World class
                ActionIds.Add(i);
            } 
        }
    }
}
