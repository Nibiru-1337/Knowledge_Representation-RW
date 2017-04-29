using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("RW-tests")]
namespace RW_backend
{
    public static class WorldOperations
    {
        public static List<OrderedDictionary> GenerateWorldNodes(OrderedDictionary fluents)
        {
            // number of nodes in graph
            int totalNodes = (int)Math.Pow(2, fluents.Count);
            List<OrderedDictionary> nodes = new List<OrderedDictionary>();
            //set fluent values in nodes
            for (uint node = 0; node < totalNodes; node++)
            {
                OrderedDictionary fluentValues = new OrderedDictionary();
                for (int keyIdx = 0; keyIdx < fluents.Count; keyIdx++)
                {
                    var item = fluents.Cast<DictionaryEntry>().ElementAt(keyIdx);
                    var bit = (node & (1 << keyIdx)) != 0;
                    fluentValues.Add(item.Key, bit);
                }
                nodes.Add(fluentValues);
            }
            return nodes;
        }

        public static List<OrderedDictionary> Resolution(RwAction action, OrderedDictionary StartingState, List<OrderedDictionary> nodes)
        {
            //check if state satisfies conditions
            foreach (var condition in action.Conditions)
            {
                if ((bool) StartingState[condition.Key] != condition.Value)
                    throw new ArgumentException("Can't execute action in this state!");
            }
            //get all states that have proper result fluents as caused by action
            List<OrderedDictionary> PossibleResults = new List<OrderedDictionary>();
            foreach (var node in nodes)
            {
                bool match = true;
                foreach (var result in action.Results)
                {
                    if ((bool) node[result.Key] != result.Value)
                    {
                        match = false;
                        break;
                    }
                }
                if (match) PossibleResults.Add(node);
            }

            //find state(s) that have the least amount of changed fluents
            List<OrderedDictionary> ConnectedStates = new List<OrderedDictionary>();
            int min = Int32.MaxValue;
            foreach (var EndingState in PossibleResults)
            {
                int diff = _StateDifference(StartingState, EndingState);
                if (min > diff)
                {
                    min = diff;
                    ConnectedStates.Clear();
                    ConnectedStates.Add(EndingState);
                }
                else if (min == diff)
                {
                    ConnectedStates.Add(EndingState);
                }
            }
            return ConnectedStates;
        }

        private static int _StateDifference(OrderedDictionary x, OrderedDictionary y)
        {
            int diff = 0;
            for(int i = 0; i < x.Count; i++)
            {
                if ((bool)x[i] != (bool)y[i]) diff++;
            }
            return diff;
        }
        static void Main(string[] args){}

    }
}
