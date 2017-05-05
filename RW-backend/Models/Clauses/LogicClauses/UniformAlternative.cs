using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	/// <summary>
	/// Klasa reprezentująca alternatywę
	/// </summary>
	public class UniformAlternative:UniformLogicClause
	{
		public override bool CheckForState(int state)
		{
			int nonnegated = state & PositiveFluents;
			if (nonnegated != 0)
				return true;

			int negated = (~state) & NegatedFluents;
			if (negated != 0)
				return true;
			return false;
		}

		
	}
}
