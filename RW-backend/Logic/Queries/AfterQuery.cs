using System;
using System.Collections.Generic;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.World;

namespace RW_backend.Logic.Queries
{
	public class AfterQuery : Query
	{

		public override QueryType Type => QueryType.After;

		
		
		public LogicClause Effect { get; } // alfa


		public AfterQuery(IReadOnlyList<ActionAgentsPair> program, LogicClause initialState, bool always, LogicClause effect)
			: base(program, initialState, always)
		{
			Effect = effect;
		}


		public override QueryResult Evaluate(World world)
		{
			MinimiserOfChanges minimiser = new MinimiserOfChanges();
			ProgramExecutionResult result = ExecuteProgram(world, minimiser,
				GetInitialStates(world.InitialStates, world.States));

			return GetResult(result);
		}


		private QueryResult GetResult(ProgramExecutionResult result)
		{
			bool allOk = true;
			bool oneOk = false;

			if (result.Executable == Executable.Never) // jeœli always, to ok
			{
				return Always 
					? new QueryResult()
					{
						IsTrue = true,
						WrongPath = result.WrongPath,
					} 
					: new QueryResult()
					{
						IsTrue = false,
						WrongPath = result.WrongPath,
					};
			}

			Console.WriteLine("reachable states = " + String.Join(", ", result.ReachableStates));
			Logger.Log("reachable states = " + String.Join(", ", result.ReachableStates));
			//Console.WriteLine("reachable states = " + String.Join(", ", result.ReachableStates));
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
			Logger.Log("result = all ok ? " + allOk + ", one ok? " + oneOk);
			return new QueryResult()
			{
				IsTrue = Always ? (allOk && result.Executable == Executable.Always) : oneOk,
				SuccessfulPath = result.SuccessfulPath,
				WrongPath = result.WrongPath
			};
		}
	}
}