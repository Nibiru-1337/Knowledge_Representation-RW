using System.Collections.Generic;
using RW_backend.Models.BitSets;

namespace RW_backend.Models.World
{
	public class ReleasesWithAgentsSet:AgentSetContainer
	{

		public BitSet FluentsReleased { get; }


		public ReleasesWithAgentsSet(int agentSet, BitSet fluentsReleased) : base(agentSet)
		{
			FluentsReleased = fluentsReleased;
		}

	}
}