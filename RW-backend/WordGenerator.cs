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
    public static class WordGenerator
    {
        public static OrderedDictionary[] GenerateWorldNodes(OrderedDictionary fluents, List<RwAction> Actions)
        {
            // number of nodes in graph
            uint TotalNodes = (uint)Math.Pow(2, fluents.Count);
            // graph representation = table of string->bool dictionaries
            OrderedDictionary[] Nodes = new OrderedDictionary[TotalNodes];
            //set fluent values in nodes
            for (uint node = 0; node < TotalNodes; node++)
            {
                Nodes[node] = new OrderedDictionary();
                for (int keyIdx = 0; keyIdx < fluents.Count; keyIdx++)
                {
                    var item = fluents.Cast<DictionaryEntry>().ElementAt(keyIdx);
                    var bit = (node & (1 << keyIdx)) != 0;
                    Nodes[node].Add(item.Key, bit);
                }
            }
            return Nodes;
        }

        static void Main(string[] args){}

    }
}
