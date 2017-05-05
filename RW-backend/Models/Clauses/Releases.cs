using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_backend.Models.Clauses
{
	/// <summary>
	/// Reprezentacja zdania "A by G releases f"
	/// </summary>
	public class Releases
	{
		public LogicClause Condition { get; }
		public int FluentReleased { get; }
		public int Action { get; }
		public AgentsSet AgentsSet { get; }

		public Releases(LogicClause condition, int fluentReleased, int action, AgentsSet agentsSet)
		{
			Condition = condition;
			FluentReleased = fluentReleased;
			Action = action;
			AgentsSet = agentsSet;
		}
	}
}