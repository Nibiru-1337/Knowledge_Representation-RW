using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	public class Conjunction:LogicClause
	{
		public override bool CheckForState(int state)
		{
			Console.WriteLine("starting...");
			Console.WriteLine("state, positive, nonegated =");
			WriteOut(state);
			WriteOut(PositiveFluents);
			int nonnegated = state & PositiveFluents;
			WriteOut(nonnegated);
			Console.WriteLine("get = " + nonnegated);
			if (nonnegated != PositiveFluents)
				return false;
			Console.WriteLine("positive = ok");
			int negated = (~state) & NegatedFluents;
			Console.WriteLine("neg-state, negatedfs, negated =");
			WriteOut(~state);
			WriteOut(NegatedFluents);
			WriteOut(negated);
			if (negated != NegatedFluents)
				return false;
			Console.WriteLine("negated = ok");
			return true;
		}

		private void WriteOut(int value)
		{
			// TODO: delete after debug
			BitValueOperator bop = new BitValueOperator();
			for (int i = 0; i < sizeof(int) * 8; i++)
			{
				Console.Write((bop.GetValue(value, i) ? "1" : "0"));
			}
			Console.WriteLine(" = " + value);

		}

	}
}
