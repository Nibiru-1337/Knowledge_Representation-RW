using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	public class AlternativeOfConjunctions:LogicClause
	{
		private readonly IList<Conjunction> _conjunctions;

		private IReadOnlyList<Conjunction> Conjunctions => _conjunctions.ToList().AsReadOnly();


		public AlternativeOfConjunctions()
		{
			_conjunctions = new List<Conjunction>();
		}

		public override bool CheckForState(int state)
		{
			return _conjunctions.Any(conjunction => conjunction.CheckForState(state));
		}

		public void AddConjunction(Conjunction conjunction)
		{
			_conjunctions.Add(conjunction);
		}
	}
}
