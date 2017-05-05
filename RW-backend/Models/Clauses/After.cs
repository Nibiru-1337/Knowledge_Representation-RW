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
		// key - akcja, value - zbiór agentów
		// mo¿e zrobiæ osobn¹ klasê do tego? bêdzie pewnie ³atwiej u¿ywaæ, a pewnie siê przyda w kilku miejscach


		public After(LogicClause effect, IReadOnlyList<ActionAgentsPair> program)
		{
			Effect = effect;
			Program = program;
		}

	}
}