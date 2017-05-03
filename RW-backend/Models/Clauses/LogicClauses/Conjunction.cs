using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	public class Conjunction:SimpleLogicClause
	{
		public override bool CheckForState(int state)
		{
			int nonnegated = state ^ PositiveFluents;
			if (nonnegated != PositiveFluents)
				return false;
			int negated = (~state) ^ NegatedFluents;
			if (negated != NegatedFluents)
				return false;
			return true;
		}
	}
}
