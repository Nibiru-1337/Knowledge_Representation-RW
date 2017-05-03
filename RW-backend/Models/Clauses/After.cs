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
		public IReadOnlyList<KeyValuePair<int, int>> ActionAgentsList { get; } // key - akcja, value - zbi�r agent�w
																			   // mo�e zrobi� osobn� klas� do tego? b�dzie pewnie �atwiej u�ywa�, a penwie si� przyda w kilku miejscach


		public After(LogicClause effect, IReadOnlyList<KeyValuePair<int, int>> actionAgentsList)
		{
			Effect = effect;
			ActionAgentsList = actionAgentsList;
		}

	}
}