using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models
{
	/// <summary>
	/// Dlaczego to, zamiast klasy abstrakcyjnej "BitValue"?
	/// Otóż że jak będziemy mieć mnóstwo czegoś
	/// to lepiej mieć mnóstwo intów niż mnóstwo instancji klas
	/// skoro chcemy, żeby to było szybkie
	/// zresztą... 
	/// </summary>
	public class BitValueOperator
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

	}
}
