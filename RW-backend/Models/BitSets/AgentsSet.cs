using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.BitSets
{
	public class AgentsSet:BitSet
	{
		public AgentsSet(int fluentValues) : base(fluentValues) {}
		public bool AgentPresent(int agentId) => this.ElementValue(agentId);
		public int AgentSet => this.Set;
	}
}
