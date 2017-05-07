using System;
using System.Collections.Generic;
using System.Linq;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.GraphModels;
using RW_backend.Models.World;

namespace RW_backend.Logic.Queries
{
	class AfterQuery : Query
	{

		public override QueryType Type => QueryType.After;

		
		
		public LogicClause Effect { get; } // alfa


		public AfterQuery(bool always, LogicClause initialState, IReadOnlyList<ActionAgentsPair> program, LogicClause effect)
			: base(program, always, initialState)
		{
			Effect = effect;
		}


		public override QueryResult Evaluate(World world)
		{
			ProgramExecutionResult result = this.ExecuteProgram(world, GetInitialStates(world.InitialStates));

			bool allOk = true;
			bool oneOk = false;

			if (result.Executable == Executable.Never)
				return new QueryResult() {IsTrue = false};

			foreach (State reachableState in result.ReachableStates)
			{
				if (Effect.CheckForState(reachableState.FluentValues))
				{
					oneOk = true;
				}
				else
				{
					allOk = false;
				}
			}
			return new QueryResult()
			{
				IsTrue = Always ? allOk : oneOk,
			};
		}
	}
}