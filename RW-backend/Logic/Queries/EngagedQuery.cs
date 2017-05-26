using System.Collections.Generic;
using System.Linq;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;
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
			var resultWithout = ExecuteProgram(world, minimiser, initialStates, AgentsSet.AgentBitSet);
			bool setsTheSame;
			bool setsExactlyDifferent;

			CompareSets(resultWith.ReachableStates,
				resultWithout.ReachableStates, out setsTheSame, out setsExactlyDifferent);

			return new QueryResult()
			{
				IsTrue = Always ? setsExactlyDifferent : !setsTheSame
			};
		}


		private void CompareSets(List<State> with, List<State> without, out bool theSame, out bool exactlyDifferent)
		{
			theSame = true;
			int howManyDifferent = 0;
			
			if (with.Count != without.Count)
				theSame = false;

			Dictionary<int, State> dictionary = with.ToDictionary(w => w.FluentValues);
			foreach (State state in without)
			{
				if (!dictionary.ContainsKey(state.FluentValues))
				{
					theSame = false;
					howManyDifferent++;
				}
			}
			if (with.Count == 0 && without.Count == 0)
				exactlyDifferent = false;
			else
				exactlyDifferent = howManyDifferent == without.Count;
		}

	}
}