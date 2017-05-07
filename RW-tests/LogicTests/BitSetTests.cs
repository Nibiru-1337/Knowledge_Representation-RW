using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models.BitSets;

namespace RW_tests.LogicTests
{
	[TestClass]
	public class BitSetTests
	{
		[TestMethod]
		public void IsSubsetOfSetTest()
		{
			var superset = new BitSet(GetSuperSet());
			var subset = new BitSet(GetSubset());
			Assert.IsTrue(subset.IsSubsetOf(superset.Set));
		}

		[TestMethod]
		public void IsSupersetOfSetTest()
		{
			var superset = new BitSet(GetSuperSet());
			var subset = new BitSet(GetSubset());
			Assert.IsTrue(superset.IsSupersetOf(subset.Set));
		}

		[TestMethod]
		public void EqualSetIsSubsetAndSupersetTest()
		{
			var first = new BitSet(GetSuperSet());
			var second = new BitSet(first.Set);
			Assert.IsTrue(first.IsSubsetOf(second.Set));
			Assert.IsTrue(first.IsSupersetOf(second.Set));
		}

		[TestMethod]
		public void EmptySetIsSubsetTest()
		{
			var empty = new BitSet(GetEmpty());
			var subset = new BitSet(GetSubset());
			Assert.IsTrue(empty.IsSubsetOf(subset.Set));
			Assert.IsTrue(subset.IsSupersetOf(empty.Set));
		}

		[TestMethod]
		public void RandomSetsSymmetryTests()
		{
			int seed = 1000;
			Random rand = new Random(seed);
			int k = 100;
			for (int i = 0; i < k; i++)
			{
				var first = new BitSet(rand.Next());
				var second = new BitSet(rand.Next());
				if (first.Set == second.Set)
				{
					Assert.IsTrue(SubsetSuperset(first, second), GetMessage(first, second));
					Assert.IsTrue(SubsetSuperset(second, first), GetMessage(first, second));
				}
				else
				{
					bool firstIsSubset = SubsetSuperset(first, second);
					bool firstIsSuperset = SubsetSuperset(second, first);
					Assert.IsFalse(firstIsSuperset && firstIsSubset, GetMessage(first, second));
				}
			}
		}

		


		private string GetMessage(BitSet first, BitSet second)
		{
			StringBuilder sb = new StringBuilder();
			var utils = new Utilities();
			sb.AppendLine();
			sb.Append(utils.BitValueToString(first.Set));
			sb.Append(utils.BitValueToString(second.Set));
			sb.Append("f subs s: ").AppendLine(first.IsSubsetOf(second.Set).ToString());
			sb.Append("s sups f: ").AppendLine(second.IsSupersetOf(first.Set).ToString());
			sb.Append("s subs f: ").AppendLine(second.IsSubsetOf(first.Set).ToString());
			sb.Append("f sups s: ").AppendLine(first.IsSupersetOf(second.Set).ToString());
			return sb.ToString();
		}
		private bool SubsetSuperset(BitSet subset, BitSet superset)
		{
			if (!subset.IsSubsetOf(superset.Set))
				return false;
			return subset.IsSupersetOf(superset.Set);
		}


		private int GetEmpty() => 0;
		private int GetSubset() => 1*4 + 1*8 + 1*32;
		private int GetSuperSet() => 1*4 + 1*8 + 1*32 + 1*16 + 1*64;
	}
}
