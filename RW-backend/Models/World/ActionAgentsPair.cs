using RW_backend.Models.BitSets;

namespace RW_backend.Models.World
{
	public class ActionAgentsPair
	{
		public int ActionId { get; }
		public AgentsSet AgentsSet { get; }

		public ActionAgentsPair(int actionId, int agentsSet)
		{
			ActionId = actionId;
			AgentsSet = new AgentsSet(agentsSet);
		}
	}
}
