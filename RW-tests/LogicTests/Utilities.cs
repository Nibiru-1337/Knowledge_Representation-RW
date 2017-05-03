using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW_backend.Models;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_tests.LogicTests
{
	class Utilities
	{
		public void SetFluents(List<int> fluents, bool negated, SimpleLogicClause clause)
		{
			foreach (int fluent in fluents)
			{
				clause.AddFluent(fluent, negated);
			}
		}

		public State GetState(List<int> fs)
		{
			int state = 0;
			BitValueOperator bop = new BitValueOperator();
			foreach (int f in fs)
			{
				state = bop.SetFluent(state, f);
			}
			return new State(state);
		}

		public void WriteOutBitValue(int value)
		{
			BitValueOperator bop = new BitValueOperator();
			for (int i = 0; i < sizeof(int) * 8; i++)
			{
				Console.Write((bop.GetValue(value, i) ? "1" : "0"));
			}
			Console.WriteLine(" = " + value);
		}



		public class Pair
		{
			public int Fluent;
			public bool Negated;

			public Pair(int f, bool n)
			{
				Fluent = f;
				Negated = n;
			}
		}

	}
}
