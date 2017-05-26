using RW_backend.Models.Factories;

namespace RW_backend.Models.BitSets
{
	public class AgentsSet:BitSet
	{
		public AgentsSet(int fluentValues) : base(fluentValues) {}
		public AgentsSet(AgentsSet set) : base(set.AgentBitSet) { }
		public bool AgentPresent(int agentId) => ElementValue(agentId);
		public int AgentBitSet => Set;

		public static AgentsSet CreateFromOneAgent(int agentId)
		{
			return new AgentsSet(new BitSetFactory().CreateFromOneElement(agentId));
		}


	}
}
