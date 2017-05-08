using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW_backend.Models.BitSets;

namespace RW_backend.Models.Factories
{
	public class BitSetFactory
	{
		BitValueOperator _bitValueOperator = new BitValueOperator();
		
		public int CreateBitSetValueFrom(List<int> elements)
		{
			int set = 0;
			foreach (int element in elements)
			{
				set = _bitValueOperator.SetFluent(set, element);
			}
			return set;
		}

		public int CreateFromOneElement(int element)
		{
			return _bitValueOperator.SetFluent(0, element);
		}

		
	}
}
