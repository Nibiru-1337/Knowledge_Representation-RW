using System;
using System.Collections.Generic;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.GraphModels;
using RW_backend.Models.World;

namespace RW_backend.Logic.Queries
{
	class ExectutableQuery : Query
	{
		public override QueryType Type => QueryType.Executable;

		public override QueryResult Evaluate(World world)
		{
			ProgramExecutionResult result = this.ExecuteProgram(world, GetInitialStates(world.InitialStates));
			return new QueryResult()
			{
				IsTrue =
					Always
						? result.Executable == Executable.Always
						: result.Executable == Executable.Sometimes
			};
		}

		public ExectutableQuery(IReadOnlyList<ActionAgentsPair> program, bool always, LogicClause initialState)
			: base(program, always, initialState)
		{
		}
	}
}