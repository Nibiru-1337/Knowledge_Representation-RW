using RW_backend.Models.BitSets;

namespace RW_backend.Models.World
{
	public abstract class AgentSetContainer
	{
		public AgentsSet AgentsSet { get; }

		protected AgentSetContainer(int agentsSet)
		{
			AgentsSet = new AgentsSet(agentsSet);
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
