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
	public class EngagedQuery : Query
	{
		public override QueryType Type => QueryType.Engaged;
		public AgentsSet AgentsSet { get; }

		public EngagedQuery(IReadOnlyList<ActionAgentsPair> program, LogicClause initialState, bool always, AgentsSet agentsSet) 
			: base(program, initialState, always)
		{
			AgentsSet = agentsSet;
		}

		public override QueryResult Evaluate(World world)
		{
			// no wi�c robimy dwie wersje: z i bez agent�w
			var initialStates = GetInitialStates(world.InitialStates);
			MinimiserOfChanges minimiser = new MinimiserOfChanges();
			var resultWith = ExecuteProgram(world, minimiser, initialStates.ToList(), 0);
			var resultWithout = ExecuteProgram(world, minimiser, initialStates, AgentsSet.AgentSet);
			// TODO: co� trzeba zrobi� z r�nic�
			return new QueryResult()
			{
				IsTrue = resultWithout.Executable == Executable.Always,
				
			};
		}

	}
}