using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.GraphModels
{
	/// <summary>
	/// Jaki był cel stworzenia tej oto klasy? [Agata]
	/// Miała być ona do Worlda do słownika readonly Dictionary(int albo State, LinkedState)
	/// 
	/// TODO: usunąć jeśli na pewno będzie niepotrzebna
	/// </summary>
	public class LinkedState
	{
		public State State { get; }
		// pierwszy int/nowa klasa: akcjaId
		// drugi int/nowa klasa: zbiór agentów, może być obiektem klasy (nowej) AgentsSet
		// (która będzie tego inta zawierać - int znowu jest "zbiorem bitowym", 
		// tzn. jeśli jest 1 dla danego indeksu, to znaczy, że dnay obiekt o danym indeksie jest zawarty w zbiorze)
		private Dictionary<int, Dictionary<int, List<LinkedState>>> OutEdges;
		private Dictionary<int, Dictionary<int, List<LinkedState>>> InEdges;
		
		// w Worldzie: Dictionary<State, LinkedState>




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
