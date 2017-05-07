using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RW_backend.Models.BitSets;

namespace RW_backend.Models.Clauses.LogicClauses
{
	/// <summary>
	/// Klasa reprezentująca formułę logiczną zawierającą wyłącznie koniunkcje
	/// </summary>
	public class UniformConjunction:UniformLogicClause
	{
		public override bool CheckForState(int state)
		{
			if (PositiveFluents == 0 && NegatedFluents == 0) // gdy to pusta koniunkcja
				return true;

			int nonnegated = state & PositiveFluents;
			if (nonnegated != PositiveFluents)
				return false;
			
			int negated = (~state) & NegatedFluents;
			
			if (negated != NegatedFluents)
				return false;


			return true;
		}

		public static UniformConjunction CreateFrom(List<int> positive, List<int> negated)
		{
			var response = new UniformConjunction();
			response.SetFluents(positive, negated);
			return response;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			var positive = new BitSet(PositiveFluents).GetAllFromSet();
			var negated = new BitSet(NegatedFluents).GetAllFromSet();
			if (positive.Count > 0 && negated.Count > 0)
			{
				sb.Append(string.Join(" ^ ", positive))
					.Append(" ^ ")
					.Append(string.Join(" ^ ", negated.Select(p => "!" + p)));
			}
			else if (positive.Count > 0)
			{
				sb.Append(string.Join(" ^ ", positive));
			}
			else if (negated.Count > 0)
			{
				sb.Append(string.Join(" ^ ", negated.Select(p => "!" + p)));
			}
			return sb.ToString();
		}

	}
}
