using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	/// <summary>
	/// Klasa reprezentująca formułę logiczną zawierającą wyłącznie koniunkcje
	/// </summary>
	public class Conjunction:UniformLogicClause
	{
		public override bool CheckForState(int state)
		{
			int nonnegated = state & PositiveFluents;
			if (nonnegated != PositiveFluents)
				return false;
			
			int negated = (~state) & NegatedFluents;
			
			if (negated != NegatedFluents)
				return false;
			return true;
		}
		

	}
}
