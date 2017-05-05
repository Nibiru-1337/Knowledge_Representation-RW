using System.Collections;
using System.Collections.Generic;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.GraphModels;

namespace RW_backend.Models.Clauses
{
	/// <summary>
	/// Reprezentacja zdania "alfa after A1 by G1,...,An by Gn"
	/// </summary>
	public class After
	{

		public LogicClause Effect { get; } // alfa
		public IReadOnlyList<ActionAgentsPair> Program { get; } 
		// key - akcja, value - zbi�r agent�w
		// mo�e zrobi� osobn� klas� do tego? b�dzie pewnie �atwiej u�ywa�, a pewnie si� przyda w kilku miejscach


		public After(LogicClause effect, IReadOnlyList<ActionAgentsPair> program)
		{
			Effect = effect;
			Program = program;
		}

	}
}