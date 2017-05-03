using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	public class Alternative:SimpleLogicClause
	{
		public override bool CheckForState(int state)
		{
			Console.WriteLine("starting...");
			Console.WriteLine("state, positive, nonegated =");
			WriteOut(state);
			WriteOut(PositiveFluents);
			int nonnegated = state & PositiveFluents;
			WriteOut(nonnegated);
			if (nonnegated != 0)
				return true;

			int negated = (~state) & NegatedFluents;
			Console.WriteLine("neg-state, negatedfs, negated =");
			WriteOut(~state);
			WriteOut(NegatedFluents);
			WriteOut(negated);
			if (negated != 0)
				return true;
			return false;
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
