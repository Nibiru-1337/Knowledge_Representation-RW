using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Models.BitSets;

namespace RW_tests.BuildingOfWorldTests
{
	[TestClass]
	public class MinimiserTests
	{
		const int NotAliveNotLoaded = 0;
		const int AliveNotLoaded = 1;
		const int NotAliveLoaded = 2;
		const int AliveLoaded = 3;



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


		[TestMethod]
		public void MinimiseChangesToZeroTest()
		{
			MinimiserOfChanges minimser = new MinimiserOfChanges();
			var result = minimser.MinimaliseChanges(new State(AliveNotLoaded),
				new List<State>() { new State(AliveNotLoaded), new State(NotAliveNotLoaded) }, 0, 0);
			Assert.AreEqual(1, result.Count, "wrong number of minimised states");
			Assert.AreEqual(AliveNotLoaded, result[0].FluentValues, "wrong state after minimisation");


		}



	}
}
