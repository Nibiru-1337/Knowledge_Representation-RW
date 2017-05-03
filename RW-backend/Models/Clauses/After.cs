using System.Collections;
using System.Collections.Generic;

namespace RW_backend.Models.Clauses
{
	/// <summary>
	/// Reprezentacja zdania "alfa after A1 by G1,...,An by Gn"
	/// </summary>
	public class After
	{

		public LogicClause Effect { get; } // alfa
		public IReadOnlyList<KeyValuePair<int, int>> ActionAgentsList { get; } // key - akcja, value - zbiór agentów
																			   // mo¿e zrobiæ osobn¹ klasê do tego? bêdzie pewnie ³atwiej u¿ywaæ, a penwie siê przyda w kilku miejscach


		public After(LogicClause effect, IReadOnlyList<KeyValuePair<int, int>> actionAgentsList)
		{
			Effect = effect;
			ActionAgentsList = actionAgentsList;
		}

	}
}