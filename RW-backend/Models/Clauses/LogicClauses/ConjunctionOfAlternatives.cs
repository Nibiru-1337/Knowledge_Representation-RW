using System.Collections.Generic;
using System.Linq;

namespace RW_backend.Models.Clauses.LogicClauses
{
	/// <summary>
	/// Klasa reprezentująca formułę logiczną w postaci CNF
	/// </summary>
	public class ConjunctionOfAlternatives:LogicClause
	{
		private readonly List<UniformAlternative> _alternatives;
		public IReadOnlyList<UniformAlternative> Alternatives => _alternatives.ToList().AsReadOnly();

		public ConjunctionOfAlternatives()
		{
			_alternatives = new List<UniformAlternative>();
		}

		public override bool CheckForState(int state)
		{
			return _alternatives.All(conjunction => conjunction.CheckForState(state));
		}

		public override bool IsEmpty()
		{
			return _alternatives.All(conj => conj.IsEmpty());
		}


		public void AddAlternative(UniformAlternative alternative)
		{
			_alternatives.Add(alternative);
		}

		public void AddRange(List<UniformAlternative> alternatives)
		{
			_alternatives.AddRange(alternatives);
		}

		public static ConjunctionOfAlternatives CreateFrom(List<UniformAlternative> alternatives)
		{
			var response = new ConjunctionOfAlternatives();
			response.AddRange(alternatives);
			return response;
		}

		public override string ToString()
		{
			if (Alternatives.Count > 0)
			{
				return "(" + string.Join(") ^ (", Alternatives) + ")";
			}
			return "[no alts]";
		}
	}
}
