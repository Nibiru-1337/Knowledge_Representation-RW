using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.GraphModels
{
	public class ActionAgentsPair
	{
		public int ActionId { get; }
		public int AgentsSet { get; }

		public ActionAgentsPair(int actionId, int agentsSet)
		{
			ActionId = actionId;
			AgentsSet = agentsSet;
		}
	}
}
