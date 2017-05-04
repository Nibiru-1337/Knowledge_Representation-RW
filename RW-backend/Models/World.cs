using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using RW_backend.Models.Clauses;

namespace RW_backend.Models
{
    /// <summary>
    /// Reprezentuje modelowany świat za pomocą grafu stanów
    /// </summary>
    public class World
    {
        //TODO implementacja grafu stanów świata, stanów początkowych itd
        //TODO WORK IN PROGESS!

        // so far the constructor creates an array of all possible states
        // without any edges in between them (edges are added with AddCauses)
        private readonly State[] _states;

        //connections b/w states are represented with a dictionary of tuples -> lists
        //tuple(key) says which Causes and which starting state
        //List<States>(value) gives us resulting states that are connected 
        public  Dictionary<Tuple<Causes,State>, List<State>> Connections { get; private set; } 
        

        public World(int fluentsCount)
        {
            int totalNodes = (int)Math.Pow(2, fluentsCount);
            Connections = new Dictionary<Tuple<Causes, State>, List<State>>();
            _states = new State[totalNodes];
            for (int i = 0; i < totalNodes; i++)
            {
                _states[i] = new State(i);
            }
        }

        //add the edges concerning a particular causes action
        public void AddCauses(Causes a)
        {
            foreach (var startingState in _states)
            {
                //if there is no conditions or state satisfies conditions
                if (a.Condition == null || a.Condition.CheckForState(startingState.FluentValues))
                {
                    //get all states that have proper result fluents as caused by action
                    List <State> possibleResults = new List<State>();
                    foreach (var endingState in _states)
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
                    //add the results of Causes from starting state
                    Connections.Add(new Tuple<Causes, State>(a, startingState), result);
                }
            }
        }

        private static int _StateDifference(State x, State y)
        {
            int diff = x.FluentValues ^ y.FluentValues;
            //return the amount of set bits in diff
            diff = diff - ((diff >> 1) & 0x55555555);
            diff = (diff & 0x33333333) + ((diff >> 2) & 0x33333333);
            return  ((diff + (diff >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
        }

    }
}
