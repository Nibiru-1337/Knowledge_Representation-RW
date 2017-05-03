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
	public class SimpleLogicClausesTests
	{



		[TestMethod]
		public void ConjunctionsPassTest()
		{
			Conjunction conjunction = new Conjunction();
			SetFluents(new List<int>() { 1, 3, 4, 5, 13 }, false, conjunction);
			SetFluents(new List<int>() { 2, 11, 18 }, true, conjunction);
			State state = GetState(new List<int>() {1, 3, 4, 5, 10, 13});
			Assert.AreEqual(true, conjunction.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void ConjunctionsWrongPositiveTest()
		{
			Conjunction conjunction = new Conjunction();
			SetFluents(new List<int>() { 1, 3, 4, 5, 13 }, false, conjunction);
			SetFluents(new List<int>() { 2, 11, 18 }, true, conjunction);
			State state = GetState(new List<int>() { 1, 3, 4, 10, 13 });
			
			Assert.AreEqual(false, conjunction.CheckForState(state.FluentValues), "Wrong value of clause");
		}


		[TestMethod]
		public void ConjunctionsWrongNegatedTest()
		{
			Conjunction conjunction = new Conjunction();
			SetFluents(new List<int>() { 1, 3, 4, 5, 13 }, false, conjunction);
			SetFluents(new List<int>() { 2, 11, 18 }, true, conjunction);
			State state = GetState(new List<int>() { 1, 3, 4, 5, 10, 13, 18 });
			Assert.AreEqual(false, conjunction.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void AlternativePassPositiveTest()
		{
			Alternative alternative = new Alternative();
			SetFluents(new List<int>() {1, 3, 4}, false, alternative);
			SetFluents(new List<int>() {7, 8}, true, alternative);
			State state = GetState(new List<int>() {1, 9, 10, 4, 7, 8});
			Assert.AreEqual(true, alternative.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void AlternativePassNegativeTest()
		{
			Alternative alternative = new Alternative();
			SetFluents(new List<int>() { 1, 3, 4 }, false, alternative);
			SetFluents(new List<int>() { 7, 8 }, true, alternative);
			State state = GetState(new List<int>() { 9, 10, });
			Assert.AreEqual(true, alternative.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void AlternativePassPositiveAndNegativeTest()
		{
			Alternative alternative = new Alternative();
			SetFluents(new List<int>() { 1, 3, 4 }, false, alternative);
			SetFluents(new List<int>() { 7, 8 }, true, alternative);
			State state = GetState(new List<int>() { 1, 4, 15});
			Assert.AreEqual(true, alternative.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		[TestMethod]
		public void AlternativeFailTest()
		{
			Alternative alternative = new Alternative();
			SetFluents(new List<int>() { 1, 3, 4 }, false, alternative);
			SetFluents(new List<int>() { 7, 8 }, true, alternative);
			State state = GetState(new List<int>() { 7, 8});
			Assert.AreEqual(false, alternative.CheckForState(state.FluentValues), "Wrong value of clause");
		}

		
		

	}
}
