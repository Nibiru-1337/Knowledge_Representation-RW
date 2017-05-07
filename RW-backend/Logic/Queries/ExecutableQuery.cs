using System;
using System.Collections.Generic;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.GraphModels;
using RW_backend.Models.World;

namespace RW_backend.Logic.Queries
{
	public class ExecutableQuery : Query
	{
		public override QueryType Type => QueryType.Executable;

		public ExecutableQuery(IReadOnlyList<ActionAgentsPair> program, LogicClause initialState, bool always)
			: base(program, initialState, always)
		{
		}

		public override QueryResult Evaluate(World world)
		{
			MinimiserOfChanges minimiser = new MinimiserOfChanges();
			var initial = GetInitialStates(world.InitialStates);
			ProgramExecutionResult result = this.ExecuteProgram(world, minimiser, initial);

			return new QueryResult()
			{
				IsTrue =
					Always
						? result.Executable == Executable.Always
						: (result.Executable == Executable.Always || result.Executable == Executable.Sometimes)
			};
		}

		
	}
}