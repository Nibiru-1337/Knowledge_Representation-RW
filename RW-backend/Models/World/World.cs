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

        //TODO implementacja grafu stanów świata, stanów początkowych itd
        //TODO WORK IN PROGESS!

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

        public World(int fluentsCount, IList<LogicClause> alwaysList, IList<LogicClause> initiallyList)
        {
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

	        InitialStates =
		        States.Where(
			        state =>
				        initiallyList.All(initially => initially.CheckForState(state.FluentValues)))
			        .ToList();
        }

        //add the edges concerning a particular causes action
        public void AddCauses(IList<Causes>causesList, int actionCount)
        {
            //for every unique ActionID
            for (int i = 0; i < actionCount; i++)
            {
                var stateToAgentSetCheckers = new Dictionary<State, IList<AgentSetChecker>>();
                //for every starting state
                foreach (var startingState in States)
                {
                
                    //TODO check assumption that actionCount is numer of unique ActionIDs, and that they are always sequential
                    List<Causes> sameID = causesList.Where(a => a.Action == i).ToList();
                    var ascList = new List<AgentSetChecker>(sameID.Count);
                    //for each action with the same ActionID
                    foreach (var a in sameID)
                    {
                        //if there is no conditions or state satisfies conditions
                        if (a.Condition == null || a.Condition.CheckForState(startingState.FluentValues))
                        {
                            //get all states that have proper result fluents as caused by action
                            List<State> possibleResults = new List<State>();
                            foreach (var endingState in States)
                            {
                                if (a.Effect.CheckForState(endingState.FluentValues))
                                {
                                    possibleResults.Add(endingState);
                                }
                            }

                            //find state(s) that have the least amount of changed fluents
                            List<State> result = new List<State>();
                            int min = Int32.MaxValue;
                            foreach (var endingState in possibleResults)
                            {
                                int diff = _StateDifference(startingState, endingState);
                                if (min > diff)
                                {
                                    min = diff;
                                    result.Clear();
                                    result.Add(endingState);
                                }
                                else if (min == diff)
                                {
                                    result.Add(endingState);
                                }
                            }
                            //add the results of action from starting state into ActionSetChecker
                            ascList.Add(new AgentSetChecker(a.AgentsSet.AgentSet, result));
                        }
                    }
                    stateToAgentSetCheckers.Add(startingState, ascList);
                }
                Connections.Add(i, stateToAgentSetCheckers);
                //TODO delete if ActionIds not needed in World class
                ActionIds.Add(i);
            } 
        }

        private static int _StateDifference(State x, State y)
        {
			// TODO: uwzględnić fluenty nieinertne
            int diff = x.FluentValues ^ y.FluentValues;
            //return the amount of set bits in diff
            diff = diff - ((diff >> 1) & 0x55555555);
            diff = (diff & 0x33333333) + ((diff >> 2) & 0x33333333);
            return  ((diff + (diff >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
        }

    }
}
