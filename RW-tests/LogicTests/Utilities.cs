using System;
using System.Collections.Generic;
using System.Text;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_tests.LogicTests
{
	class Utilities
	{

		public void SetFluents(List<int> fluents, FluentSign sign, UniformLogicClause clause)
		{
			foreach (int fluent in fluents)
			{
				clause.AddFluent(fluent, sign);
			}
		}

		public State GetState(List<int> fs)
		{
			int state = 0;
			BitSetOperator bop = new BitSetOperator();
			foreach (int f in fs)
			{
				state = bop.SetFluent(state, f);
			}
			return new State(state);
		}

		public void WriteOutBitValue(int value)
		{
			BitSetOperator bop = new BitSetOperator();
			for (int i = 0; i < sizeof(int) * 8; i++)
			{
				Console.Write((bop.GetValue(value, i) ? "1" : "0"));
			}
			Console.WriteLine(" = " + value);
		}

		public string BitValueToString(int value)
		{
			BitSetOperator bop = new BitSetOperator();
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < sizeof(int) * 8; i++)
			{
				sb.Append((bop.GetValue(value, i) ? "1" : "0"));
			}
			sb.AppendLine(" = " + value);
			return sb.ToString();
		}


		private void GetConjunctions(out UniformConjunction first, out UniformConjunction second, out UniformConjunction third)
		{
			// (a ^ b) v (~b ^ ~d) v (e ^ f ^ g ^ ~h)
			//  0   1      1    3     4   5   6    7
			Utilities utilities = new Utilities();
			first = new UniformConjunction();
			utilities.SetFluents(new List<int>() { 0, 1 }, FluentSign.Positive, first);
			second = new UniformConjunction();
			utilities.SetFluents(new List<int>() { 1, 3 }, FluentSign.Negated, second);
			third = new UniformConjunction();
			utilities.SetFluents(new List<int>() { 4, 5, 6 }, FluentSign.Positive, third);
			utilities.SetFluents(new List<int>() { 7 }, FluentSign.Negated, third);
		}

		public AlternativeOfConjunctions GetAlternativeOfConjunctions()
		{
			UniformConjunction first, second, third;
			GetConjunctions(out first, out second, out third);
			AlternativeOfConjunctions aoc = new AlternativeOfConjunctions();
			aoc.AddConjunction(first);
			aoc.AddConjunction(second);
			aoc.AddConjunction(third);
			return aoc;
		}

		private void GetAlternatives(out UniformAlternative first, out UniformAlternative second, out UniformAlternative third)
		{
			// (a ^ b) v (~b ^ ~d) v (e ^ f ^ g ^ ~h)
			//  0   1      1    3     4   5   6    7
			Utilities utilities = new Utilities();
			first = new UniformAlternative();
			utilities.SetFluents(new List<int>() { 0, 1 }, FluentSign.Positive, first);
			second = new UniformAlternative();
			utilities.SetFluents(new List<int>() { 1, 3 }, FluentSign.Negated, second);
			third = new UniformAlternative();
			utilities.SetFluents(new List<int>() { 4, 5, 6 }, FluentSign.Positive, third);
			utilities.SetFluents(new List<int>() { 7 }, FluentSign.Negated, third);
		}

		public ConjunctionOfAlternatives GetConjunctionOfAlternatives()
		{
			UniformAlternative first, second, third;
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
