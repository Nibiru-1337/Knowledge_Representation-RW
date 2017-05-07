using System.Collections.Generic;
using System.Net.Configuration;
using RW_backend.Models.BitSets;

namespace RW_backend.Models.World
{
    public class AgentSetChecker
    {
	    public AgentsSet AgentsSet { get; }

		public List<State> Edges { get; private set; }


        public AgentSetChecker(int agentSet, List<State> x)
        {
            AgentsSet = new AgentsSet(agentSet);
            Edges = x;
        }

        /// <summary>
        /// Checks if the agent set is allowed to execute this action
        /// </summary>
        /// <param name="agentSet">The set executing the action</param>
        /// <returns>true if input is subset of action agent set, false otherwise</returns>
        public bool CanBeExecutedByAgentsSet(int agentSet)
        {
	        return AgentsSet.IsSubsetOf(agentSet);
        }

	    public bool UsesAgentFromSet(int agentsSet)
	    {
		    return !AgentsSet.HasNoneCommonElementsWith(agentsSet);
	    }

    }
}
