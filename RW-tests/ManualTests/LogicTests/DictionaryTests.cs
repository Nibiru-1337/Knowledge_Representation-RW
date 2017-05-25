using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models.BitSets;

namespace RW_tests.ManualTests.LogicTests
{
	[TestClass]
	public class DictionaryTests
	{
		[TestMethod]
		public void DictionaryTest()
		{
			int fluentSet = 2345;
			BitSetOperator bop = new BitSetOperator();
			int n = 10;
			Dictionary<State, int> dictionary = new Dictionary<State, int>();
			for (int i = 0; i < n; i++)
			{
				try
				{
					dictionary.Add(new State(fluentSet), 0);
				}
				catch (ArgumentException)
				{
					
				}
			}

			Assert.AreEqual(1, dictionary.Keys.Count, "states should be equal");
		}

	}
}
