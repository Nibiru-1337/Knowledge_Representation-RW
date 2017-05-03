using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	public abstract class SimpleLogicClause
	{

		public int PositiveFluents { get; private set; }
		public int NegatedFluents { get; private set; }



		public SimpleLogicClause()
		{
			PositiveFluents = 0;
			NegatedFluents = 0;
		}

		public void AddFluent(int fluentId, bool negated)
		{
			if (negated)
			{
				NegatedFluents = NegatedFluents | (1 << fluentId);
			}
			else
			{
				PositiveFluents = PositiveFluents | (1 << fluentId);
			}
		}

		public void DeleteFluent(int fluentId, bool negated)
		{
			if (negated)
			{
				NegatedFluents = NegatedFluents ^ (~(1 << fluentId));
			}
			else
			{
				PositiveFluents = PositiveFluents ^ (~(1 << fluentId));
			}
		}

		public abstract bool CheckForState(int state);

	}
}
