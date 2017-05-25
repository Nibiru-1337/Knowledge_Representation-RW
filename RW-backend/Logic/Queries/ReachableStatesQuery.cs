using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.World;

namespace RW_backend.Logic.Queries
{
	public class ReachableStatesQuery:Query
	{
		public ReachableStatesQuery(IReadOnlyList<ActionAgentsPair> program, LogicClause initialStateCondition, bool always) : base(program, initialStateCondition, always) {}
		public override QueryType Type => QueryType.ExecutePath;
		public override QueryResult Evaluate(World world)
		{
			var result = RunQuery(world);
			return new QueryResult()
			{
				IsTrue = Always 
					? result.Executable == Executable.Always 
					: (result.Executable == Executable.Always || result.Executable == Executable.Sometimes)
			};
		}

		public ProgramExecutionResult RunQuery(World world)
		{
			MinimiserOfChanges minimiser = new MinimiserOfChanges();
			var initial = GetInitialStates(world.InitialStates, world.States);
			return ExecuteProgram(world, minimiser, initial);
		}
	}
}
