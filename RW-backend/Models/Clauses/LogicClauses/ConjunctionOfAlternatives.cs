using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	/// <summary>
	/// Klasa reprezentująca formułę logiczną w postaci CNF
	/// </summary>
	public class ConjunctionOfAlternatives:LogicClause
	{
		private readonly IList<UniformAlternative> _alternatives;
		public IReadOnlyList<UniformAlternative> Alternatives => _alternatives.ToList().AsReadOnly();

		public ConjunctionOfAlternatives()
		{
			_alternatives = new List<UniformAlternative>();
		}

		public override bool CheckForState(int state)
		{
			return _alternatives.All(conjunction => conjunction.CheckForState(state));
		}


		public void AddAlternative(UniformAlternative alternative)
		{
			_alternatives.Add(alternative);
		}

	}
}
