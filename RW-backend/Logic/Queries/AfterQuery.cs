using System;
using System.Collections.Generic;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.GraphModels;

namespace RW_backend.Models.Queries
{
	class AfterQuery : Query
	{
		public override QueryType Type => QueryType.After;

		public LogicClause Effect { get; } // alfa
		public IReadOnlyList<ActionAgentsPair> ActionAgentsList { get; }






		public override QueryResult Evaluate(World world)
		{
			throw new NotImplementedException();
		}
	}
}