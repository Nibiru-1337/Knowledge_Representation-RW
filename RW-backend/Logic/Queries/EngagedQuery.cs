using System.Collections.Generic;
using System.Linq;
using RW_backend.Logic.Queries.Results;
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
			var initialStates = GetInitialStates(world.InitialStates, world.States);
			MinimiserOfChanges minimiser = new MinimiserOfChanges();
			var resultWith = ExecuteProgram(world, minimiser, initialStates.ToList(), 0);
			var resultWithout = ExecuteProgram(world, minimiser, initialStates, AgentsSet.AgentSet);
			bool setsTheSame = CompareSets(resultWith.ReachableStates,
				resultWithout.ReachableStates);
			return new QueryResult()
			{
				IsTrue = setsTheSame,
			};
		}

		private bool CompareSets(List<State> with, List<State> without)
		{
			// zbiory musz¹ byæ takie same
			if (with.Count != without.Count)
				return false;
			Dictionary<int, State> dictionary = with.ToDictionary(w => w.FluentValues);
			foreach (State state in without)
			{
				if (!dictionary.ContainsKey(state.FluentValues))
				{
					return false;
				}
			}
			return true;
		}

	}
}