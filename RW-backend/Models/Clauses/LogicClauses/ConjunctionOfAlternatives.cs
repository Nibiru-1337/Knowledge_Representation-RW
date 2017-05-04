using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	public class ConjunctionOfAlternatives:LogicClause
	{
		private readonly IList<Alternative> _alternatives;
		public IReadOnlyList<Alternative> Alternatives => _alternatives.ToList().AsReadOnly();

		public ConjunctionOfAlternatives()
		{
			_alternatives = new List<Alternative>();
		}

		public override bool CheckForState(int state)
		{
			return _alternatives.All(conjunction => conjunction.CheckForState(state));
		}


		public void AddAlternative(Alternative alternative)
		{
			_alternatives.Add(alternative);
		}

	}
}
