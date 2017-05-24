using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_tests.LogicTests
{
	[TestClass]
	public class ConjunctionOfAlternativesTests
	{
		

		// CoA:
		// (a v b) ^ (~b v ~d) ^ (e v f v g v ~h)
		//  0   1     ~1    ~3     4   5   6  ~7
		

		[TestMethod]
		public void ConjunctionOfAlternativesPassClauseTest()
		{
			// state ok for first bc 0
			// ok for second bc bc ~1
			// ok for third, bc 4
			Utilities utilites = new Utilities();
			State state = utilites.GetState(new List<int>() { 0, 4 });
			ConjunctionOfAlternatives aoc = utilites.GetConjunctionOfAlternatives();
			Assert.AreEqual(true, aoc.CheckForState(state.FluentValues));
		}

		[TestMethod]
		public void ConjunctionOfAlternativesFailFirstClauseTest()
		{
			// state not ok for first tj ~0 ^ ~1
			// ok for second bc bc ~1
			// ok for third, bc 4
			Utilities utilites = new Utilities();
			State state = utilites.GetState(new List<int>() { 4 });
			ConjunctionOfAlternatives aoc = utilites.GetConjunctionOfAlternatives();
			Assert.AreEqual(false, aoc.CheckForState(state.FluentValues));
		}

		[TestMethod]
		public void ConjunctionOfAlternativesFailThirdClauseTest()
		{
			// state ok for first bc 0
			// ok for second bc bc ~1
			// not ok for third bc ~4 ~5 ~6 7
			Utilities utilites = new Utilities();
			State state = utilites.GetState(new List<int>() { 0, 7 });
			ConjunctionOfAlternatives aoc = utilites.GetConjunctionOfAlternatives();
			Assert.AreEqual(false, aoc.CheckForState(state.FluentValues));
		}

		[TestMethod]
		public void ConjunctionOfAlternativesFailSecondAndThirdClauseTest()
		{
			// state ok for first bc 0
			// not ok for second bc 1 ^ 3
			// not ok for third bc ~4 ~5 ~6 7
			Utilities utilites = new Utilities();
			State state = utilites.GetState(new List<int>() { 1, 3, 0, 7 });
			ConjunctionOfAlternatives aoc = utilites.GetConjunctionOfAlternatives();
			Assert.AreEqual(false, aoc.CheckForState(state.FluentValues));
		}
		

	}
}
