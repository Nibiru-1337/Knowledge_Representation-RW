using System.Collections.Generic;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;

namespace RW_backend.Models
{
    public class AgentSetChecker
    {
        private int _agentSet;
        public List<State> edges { get; private set; }
        public AgentSetChecker(int agentSet, List<State> x)
        {
            _agentSet = agentSet;
            edges = x;
        }

        /// <summary>
        /// Checks if the agent set is allowed to execute this action
        /// </summary>
        /// <param name="agentSet">The set executing the action</param>
        /// <returns>true if input is subset of action agent set, false otherwise</returns>
        public bool Check(int agentSet)
        {
            if ((_agentSet & agentSet) != 0)
                return true;
            else
                return false;
        }

    }
}
