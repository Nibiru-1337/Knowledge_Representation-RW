using System.Collections.Generic;
using RW_backend.Models.BitSets;

namespace RW_backend.Models.Factories
{
	public class BitSetFactory
	{
		readonly BitSetOperator _bitValueOperator = new BitSetOperator();
		
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

        public int CreateFromStateAndSetValue(int bitValue, int bitIdx, int state)
        {
            if (bitValue == 0 && bitIdx == 0) //TODO: special case, maybe it should be considered in BitSetOperator?
                return state & ~1;
            else
                return _bitValueOperator.SetFluent(bitValue, bitIdx) | state;
        }
    }
}
