using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models;


namespace RW_tests.LogicTests
{
	[TestClass]
	public class LogicClausesTests
	{
		// AoC:
		// (a ^ b) v (~b ^ ~d) v (e ^ f ^ g ^ ~h)
		//  0   1      1    3     4   5   6    7

		[TestMethod]
		public void AlternativeOfConjunctionsPassFirstClauseTest()
		{
			// state ok for first, tj. a ^ b
			// not ok for second, bc ok for first
			// not ok for third, bc not 4
			Utilities utilites = new Utilities();
			State state = utilites.GetState(new List<int>() {0, 1, 3, 6, 2});
			AlternativeOfConjunctions aoc = GetAlternativeOfConjunctions();
			Assert.AreEqual(true, aoc.CheckForState(state.FluentValues));
		}

		[TestMethod]
		public void AlternativeOfConjunctionsPassThirdClauseTest()
		{
			// state ok for third, tj. (e ^ f ^ g ^ ~h) // 4   5   6  ~7
			// not ok for first bc not a
			// not ok for second bc b
			Utilities utilites = new Utilities();
			State state = utilites.GetState(new List<int>() { 1, 4, 5, 6 });
			AlternativeOfConjunctions aoc = GetAlternativeOfConjunctions();
			Assert.AreEqual(true, aoc.CheckForState(state.FluentValues));
		}

		[TestMethod]
		public void AlternativeOfConjunctionsPassSecondAndThirdClauseTest()
		{
			// state ok for second tj ~1 ^ ~3
			// not ok for first bc ~1
			// ok for third tj 4   5   6  ~7
			Utilities utilites = new Utilities();
			State state = utilites.GetState(new List<int>() { 4, 5, 6 });
			AlternativeOfConjunctions aoc = GetAlternativeOfConjunctions();
			Assert.AreEqual(true, aoc.CheckForState(state.FluentValues));
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

		private AlternativeOfConjunctions GetAlternativeOfConjunctions()
		{
			Conjunction first, second, third;
			GetConjunctions(out first, out second, out third);
			AlternativeOfConjunctions aoc = new AlternativeOfConjunctions();
			aoc.AddConjunction(first);
			aoc.AddConjunction(second);
			aoc.AddConjunction(third);
			return aoc;
		}



	}
}
