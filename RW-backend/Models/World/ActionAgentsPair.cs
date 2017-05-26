using RW_backend.Models.BitSets;

namespace RW_backend.Models.World
{
	public class ActionAgentsPair
	{
		public int ActionId { get; }
		public AgentsSet AgentsSet { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="actionId">id of action</param>
		/// <param name="agentsSet">bit set of agents (you can use BitSetFactory or AgentsSet.CreateFromOne or anything)</param>
		public ActionAgentsPair(int actionId, int agentsSet)
		{
			ActionId = actionId;
			AgentsSet = new AgentsSet(agentsSet);
		}
	}
}
