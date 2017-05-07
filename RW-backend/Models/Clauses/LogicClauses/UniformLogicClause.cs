using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Clauses.LogicClauses
{
	/// <summary>
	/// Formuła logiczna "jednorodna" - w postaci wielu alternatyw LUB wielu koniunkcji
	/// (poniżej podstawowa funkcjonalność)
	/// dawne "SimpleLogicClause" lub "BasicLogicClause"
	/// pomyślałam, że to nazewnictwo pomoże lepiej zrozumieć, o co chodzi
	/// </summary>
	public abstract class UniformLogicClause:LogicClause
	{
		public int PositiveFluents { get; private set; }
		public int NegatedFluents { get; private set; }

		protected UniformLogicClause()
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
				NegatedFluents = NegatedFluents & (~(1 << fluentId));
			}
			else
			{
				PositiveFluents = PositiveFluents & (~(1 << fluentId));
			}
		}

		public void SetFluents(List<int> positive, List<int> negated)
		{
			foreach (int i in positive)
			{
				AddFluent(i, false);
			}
			foreach (int i in negated)
			{
				AddFluent(i, true);
			}
		}
	}
}
