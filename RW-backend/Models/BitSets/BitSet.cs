using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW_backend.Models.GraphModels;

namespace RW_backend.Models.BitSets
{
	public class BitSet
	{
		private int MaxElementsCount = 32;
		public int Set { get; }

		//TODO: w tej chwili, abu uzyskać tę liczbę, można skorzystać z BitValueOperator
		public BitSet(int fluentValues)
		{
			Set = fluentValues;
		}



		public bool ElementValue(int elementNumber) => (Set & (1 << elementNumber)) > 0;

		public override bool Equals(object obj)
		{
			var other = obj as BitSet;
			return Set == other?.Set;
		}

		public bool IsSubsetOf(int superset)
		{
			return (Set & superset) == Set;
		}
		public bool IsSupersetOf(int subset)
		{
			return (Set & subset) == subset;
		}

		public bool HasNoneCommonElementsWith(int otherSet)
		{
			return (otherSet & Set) == 0;
		}

		public int SetOfDifferentValuesThan(int otherSet)
		{
			return otherSet ^ Set;
		}

		public List<int> GetAllFromSet()
		{
			List<int> response = new List<int>(MaxElementsCount);
			for (int i = 0; i < MaxElementsCount; i++)
			{
				if(this.ElementValue(i))
					response.Add(i);
			}
			return response;
		}

		public override int GetHashCode()
		{
			return Set.GetHashCode();
		}

		public override string ToString()
		{
			return Set.ToString();
		}
	}
}
