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
		public int AgentsSet { get; }

		public Releases(LogicClause condition, int fluentReleased, int action, int agentsSet)
		{
			Condition = condition;
			FluentReleased = fluentReleased;
			Action = action;
			AgentsSet = agentsSet;
		}
	}
}