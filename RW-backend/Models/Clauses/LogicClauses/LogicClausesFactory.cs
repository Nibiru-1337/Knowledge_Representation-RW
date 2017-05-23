using System.Collections.Generic;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_backend.Models.Factories
{
	public class LogicClausesFactory
	{

		public LogicClause CreateEmptyLogicClause()
		{
			return new UniformAlternative();
		}

		public LogicClause CreateSingleFluentClause(int fluent, bool negated)
		{
			if (negated)
				return UniformAlternative.CreateFrom(new List<int>(), new List<int> { fluent });
			else
			{
				return UniformAlternative.CreateFrom(new List<int> { fluent }, new List<int>());
			}
		}


		public LogicClause CreateContradictingClause(int fluent = 0)
		{
			return UniformConjunction.CreateFrom(new List<int> {fluent}, new List<int> {fluent});
		}

	}
}
