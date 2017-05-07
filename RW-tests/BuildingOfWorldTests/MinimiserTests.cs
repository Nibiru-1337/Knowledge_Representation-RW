using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Models.BitSets;

namespace RW_tests.BuildingOfWorldTests
{
	[TestClass]
	public class MinimiserTests
	{
		// TODO: tests with noninertial fluents
		// TODO: tests with releases
		[TestMethod]
		public void MinimiseStatesTest()
		{
			List<State> available = new List<State>()
			{
				new State(0), new State(1), new State(2), new State(3),
			};
			State initial = new State(7); // 111 // czyli najbliżej: 011, czyli 3
			List<State> afterMinimisation = new MinimiserOfChanges().MinimaliseChanges(initial,
				available);
			Assert.AreEqual(1, afterMinimisation.Count, "wrong count of states after minimisation");
			Assert.AreEqual(3, afterMinimisation[0].FluentValues, "wrong state after minimisation");
		}




	}
}
