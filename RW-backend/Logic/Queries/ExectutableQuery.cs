using System;
using System.Collections.Generic;
using RW_backend.Models;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.GraphModels;

namespace RW_backend.Logic.Queries
{
	class ExectutableQuery : Query
	{
		public LogicClause InitialState { get; } // alfa
		public IReadOnlyList<ActionAgentsPair> Program { get; }



		public override QueryType Type => QueryType.Executable;

		public override QueryResult Evaluate(World world)
		{
			throw new NotImplementedException();
		}
	}
}