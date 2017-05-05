using System;
using System.Collections.Generic;
using RW_backend.Models;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.GraphModels;

namespace RW_backend.Logic.Queries
{
	class AfterQuery : Query
	{
		public override QueryType Type => QueryType.After;

		
		public LogicClause InitialState { get; } // alfa
		public IReadOnlyList<ActionAgentsPair> Program { get; }
		public LogicClause Effect { get; } // alfa





		public override QueryResult Evaluate(World world)
		{
			throw new NotImplementedException();
		}
	}
}