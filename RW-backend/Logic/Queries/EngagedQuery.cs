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

			int withCount = with?.Count ?? 0;
			int withoutCount = without?.Count ?? 0;

		    if (withCount == 0 && withoutCount == 0)
		    {
		        exactlyDifferent = false;
		        return;
		    }

		    if (withCount != withoutCount)
				theSame = false;

			Dictionary<int, State> dictionary = withCount == 0 
				? new Dictionary<int, State>() 
				: with.ToDictionary(w => w.FluentValues);

			if (withoutCount != 0)
			{
				foreach (State state in without)
				{
					if (!dictionary.ContainsKey(state.FluentValues))
					{
						theSame = false;
						howManyDifferent++;
					}
				}
			}
			
			exactlyDifferent = howManyDifferent == withoutCount;
		}

	}
}