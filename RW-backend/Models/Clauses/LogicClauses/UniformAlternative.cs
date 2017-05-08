using System.Collections.Generic;
using System.Linq;
using System.Text;
using RW_backend.Models.BitSets;

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
			if (nonnegated != 0) // przy założeniu, że jakieś Positive istnieją
				return true;

			int negated = (~state) & NegatedFluents; // przy założeniu, że jakieś Negated istnieją
			if (negated != 0)
				return true;

			if (PositiveFluents == 0 && NegatedFluents == 0) // gdy to pusta alternatywa
				return true;
			return false;
		}


		public static UniformAlternative CreateFrom(List<int> positive, List<int> negated)
		{
			var response = new UniformAlternative();
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
				sb.Append(string.Join(" v ", positive))
					.Append(" v ")
					.Append(string.Join(" v ", negated.Select(p => "!" + p)));
			}
			else if (positive.Count > 0)
			{
				sb.Append(string.Join(" v ", positive));
			}
			else if (negated.Count > 0)
			{
				sb.Append(string.Join(" v ", negated.Select(p => "!" + p)));
			}
			return sb.ToString();
		}
	}
}
