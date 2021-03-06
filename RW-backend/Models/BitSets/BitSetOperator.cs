﻿namespace RW_backend.Models.BitSets
{
	/// <summary>
	/// operacje na bitach
	/// </summary>
	public class BitSetOperator
	{
        public int SetFluent(int bitValue, int bitIndex)
		{
			return bitValue | (1 << bitIndex);
		}

		public int SetNegatedFluent(int bitValue, int bitIndex)
		{
			return bitValue ^ (~(1 << bitIndex));
		}

		public bool GetValue(int bitValue, int bitIndex)
		{
			return (bitValue & (1 << bitIndex)) > 0;
		}

		public int GetSumOfSets(int left, int right)
		{
			return left | right;
		}

	}
}
