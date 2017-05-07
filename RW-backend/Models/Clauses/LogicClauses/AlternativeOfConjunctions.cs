using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	/// <summary>
	/// Klasa reprezentująca formułę logiczną w postaci DNF
	/// </summary>
	public class AlternativeOfConjunctions:LogicClause
	{
		private readonly List<UniformConjunction> _conjunctions;

		public IReadOnlyList<UniformConjunction> Conjunctions => _conjunctions.ToList().AsReadOnly();


		public AlternativeOfConjunctions()
		{
			_conjunctions = new List<UniformConjunction>();
		}

		public override bool CheckForState(int state)
		{
			return _conjunctions.Any(conjunction => conjunction.CheckForState(state));
		}

		public void AddConjunction(UniformConjunction uniformConjunction)
		{
			_conjunctions.Add(uniformConjunction);
		}

		public void AddRange(List<UniformConjunction> conjunctions)
		{
			_conjunctions.AddRange(conjunctions);
		}

		public static AlternativeOfConjunctions CreateFrom(List<UniformConjunction> conjunctions)
		{
			var response = new AlternativeOfConjunctions();
			response.AddRange(conjunctions);
			return response;
		}

		public override string ToString()
		{
			if (Conjunctions.Count > 0)
			{
				return "(" + string.Join(") v (", Conjunctions) + ")";
			}
			return "[no conjs]";
		}
	}
}
