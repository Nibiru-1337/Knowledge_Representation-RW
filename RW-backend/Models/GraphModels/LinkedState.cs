using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.GraphModels
{
	public class LinkedState
	{
		public State State { get; }
		// nie mamy pewności, czy tak powinny wyglądać krawędzie, no ale niech będzie
		private Dictionary<int, Dictionary<int, List<LinkedState>>> OutEdges;
		private Dictionary<int, Dictionary<int, List<LinkedState>>> InEdges;


		public List<LinkedState> GetOutStates(int actionId, int agentsSet)
		{
			if(OutEdges.ContainsKey(actionId))
				if (OutEdges[actionId].ContainsKey(agentsSet))
					return OutEdges[actionId][agentsSet];
			return null;
		}


		public List<LinkedState> GetInStates(int actionId, int agentsSet)
		{
			if (InEdges.ContainsKey(actionId))
				if (InEdges[actionId].ContainsKey(agentsSet))
					return InEdges[actionId][agentsSet];
			return null;
		}


	}
}
