using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;

namespace RW_tests.LogicTests
{
	[TestClass]
	public class UniformLogicClausesTests
	{



		[TestMethod]
		public void ConjunctionsPassTest()
		{
			Utilities utilities = new Utilities();
			UniformConjunction conjunction = new UniformConjunction();
			utilities.SetFluents(new List<int>() { 1, 3, 4, 5, 13 }, false, conjunction);
			utilities.SetFluents(new List<int>() { 2, 11, 18 }, true, conjunction);
			State state = utilities.GetState(new List<int>() {1, 3, 4, 5, 10, 13});
			Assert.AreEqual(true, conjunction.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void ConjunctionsWrongPositiveTest()
		{
			Utilities utilities = new Utilities();
			UniformConjunction conjunction = new UniformConjunction();
			utilities.SetFluents(new List<int>() { 1, 3, 4, 5, 13 }, false, conjunction);
			utilities.SetFluents(new List<int>() { 2, 11, 18 }, true, conjunction);
			State state = utilities.GetState(new List<int>() { 1, 3, 4, 10, 13 });
			
			Assert.AreEqual(false, conjunction.CheckForState(state.FluentValues), "Wrong value of clause");
		}


		[TestMethod]
		public void ConjunctionsWrongNegatedTest()
		{
			Utilities utilities = new Utilities();
			UniformConjunction conjunction = new UniformConjunction();
			utilities.SetFluents(new List<int>() { 1, 3, 4, 5, 13 }, false, conjunction);
			utilities.SetFluents(new List<int>() { 2, 11, 18 }, true, conjunction);
			State state = utilities.GetState(new List<int>() { 1, 3, 4, 5, 10, 13, 18 });
			Assert.AreEqual(false, conjunction.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void AlternativePassPositiveTest()
		{
			Utilities utilities = new Utilities();
			UniformAlternative alternative = new UniformAlternative();
			utilities.SetFluents(new List<int>() {1, 3, 4}, false, alternative);
			utilities.SetFluents(new List<int>() {7, 8}, true, alternative);
			State state = utilities.GetState(new List<int>() {1, 9, 10, 4, 7, 8});
			Assert.AreEqual(true, alternative.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void AlternativePassNegativeTest()
		{
			Utilities utilities = new Utilities();
			UniformAlternative alternative = new UniformAlternative();
			utilities.SetFluents(new List<int>() { 1, 3, 4 }, false, alternative);
			utilities.SetFluents(new List<int>() { 7, 8 }, true, alternative);
			State state = utilities.GetState(new List<int>() { 9, 10, });
			Assert.AreEqual(true, alternative.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void AlternativePassPositiveAndNegativeTest()
		{
			Utilities utilities = new Utilities();
			UniformAlternative alternative = new UniformAlternative();
			utilities.SetFluents(new List<int>() { 1, 3, 4 }, false, alternative);
			utilities.SetFluents(new List<int>() { 7, 8 }, true, alternative);
			State state = utilities.GetState(new List<int>() { 1, 4, 15});
			Assert.AreEqual(true, alternative.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void AlternativeFailTest()
		{
			Utilities utilities = new Utilities();
			UniformAlternative alternative = new UniformAlternative();
			utilities.SetFluents(new List<int>() { 1, 3, 4 }, false, alternative);
			utilities.SetFluents(new List<int>() { 7, 8 }, true, alternative);
			State state = utilities.GetState(new List<int>() { 7, 8});
			Assert.AreEqual(false, alternative.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void LogicClauseFactoryTests()
		{
			LogicClausesFactory factory = new LogicClausesFactory();
			int fluentsNumber = 10;
			int lastState = (1 << fluentsNumber);
			var emptyClause = factory.CreateEmptyLogicClause();
			for (int i = 0; i < lastState; i++)
			{
				Assert.IsTrue(emptyClause.CheckForState(i), "failed for state = " + i);
			}
		}


		[TestMethod]
		public void LogicClauseFactoryContradictingTests()
		{
			LogicClausesFactory factory = new LogicClausesFactory();
			int fluentsNumber = 10;
			int lastState = (1 << fluentsNumber);
			var contrClause = factory.CreateContradictingClause();
			for (int i = 0; i < lastState; i++)
			{
				Assert.IsFalse(contrClause.CheckForState(i), "failed for state = " + i);
			}
		}

	}
}
