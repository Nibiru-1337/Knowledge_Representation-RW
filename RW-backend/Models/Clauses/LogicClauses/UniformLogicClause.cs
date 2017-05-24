using System.Collections.Generic;

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

		public void AddFluent(int fluentId, FluentSign sign)
		{
			if (sign == FluentSign.Negated)
			{
				NegatedFluents = NegatedFluents | (1 << fluentId);
			}
			else
			{
				PositiveFluents = PositiveFluents | (1 << fluentId);
			}
		}

		public void DeleteFluent(int fluentId, FluentSign sign)
		{
			if (sign == FluentSign.Negated)
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
			PositiveFluents = 0;
			NegatedFluents = 0;
			foreach (int i in positive)
			{
				AddFluent(i, FluentSign.Positive);
			}
			foreach (int i in negated)
			{
				AddFluent(i, FluentSign.Negated);
			}
		}

		public void SetFluents(int positive, int negated)
		{
			PositiveFluents = positive;
			NegatedFluents = negated;
		}
	}
}
