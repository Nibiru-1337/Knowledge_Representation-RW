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
		public void SetFluents(List<int> fluents, bool negated, UniformLogicClause clause)
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


		private void GetConjunctions(out Conjunction first, out Conjunction second, out Conjunction third)
		{
			// (a ^ b) v (~b ^ ~d) v (e ^ f ^ g ^ ~h)
			//  0   1      1    3     4   5   6    7
			Utilities utilities = new Utilities();
			first = new Conjunction();
			utilities.SetFluents(new List<int>() { 0, 1 }, false, first);
			second = new Conjunction();
			utilities.SetFluents(new List<int>() { 1, 3 }, true, second);
			third = new Conjunction();
			utilities.SetFluents(new List<int>() { 4, 5, 6 }, false, third);
			utilities.SetFluents(new List<int>() { 7 }, true, third);
		}

		public AlternativeOfConjunctions GetAlternativeOfConjunctions()
		{
			Conjunction first, second, third;
			GetConjunctions(out first, out second, out third);
			AlternativeOfConjunctions aoc = new AlternativeOfConjunctions();
			aoc.AddConjunction(first);
			aoc.AddConjunction(second);
			aoc.AddConjunction(third);
			return aoc;
		}

		private void GetAlternatives(out Alternative first, out Alternative second, out Alternative third)
		{
			// (a ^ b) v (~b ^ ~d) v (e ^ f ^ g ^ ~h)
			//  0   1      1    3     4   5   6    7
			Utilities utilities = new Utilities();
			first = new Alternative();
			utilities.SetFluents(new List<int>() { 0, 1 }, false, first);
			second = new Alternative();
			utilities.SetFluents(new List<int>() { 1, 3 }, true, second);
			third = new Alternative();
			utilities.SetFluents(new List<int>() { 4, 5, 6 }, false, third);
			utilities.SetFluents(new List<int>() { 7 }, true, third);
		}

		public ConjunctionOfAlternatives GetConjunctionOfAlternatives()
		{
			Alternative first, second, third;
			GetAlternatives(out first, out second, out third);
			ConjunctionOfAlternatives aoc = new ConjunctionOfAlternatives();
			aoc.AddAlternative(first);
			aoc.AddAlternative(second);
			aoc.AddAlternative(third);
			return aoc;
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
