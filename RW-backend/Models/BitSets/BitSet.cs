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

		public int Set { get; }

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

		public override int GetHashCode()
		{
			return Set.GetHashCode();
		}
	}
}
