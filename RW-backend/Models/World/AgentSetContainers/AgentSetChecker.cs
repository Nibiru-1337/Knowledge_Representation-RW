using System.Collections.Generic;
using RW_backend.Models.BitSets;

namespace RW_backend.Models.World
{
    public class AgentSetChecker:AgentSetContainer
    {

		public List<State> Edges { get; private set; }


        public AgentSetChecker(int agentSet, List<State> x):base(agentSet)
        {
            Edges = x;
        }

    }
}
