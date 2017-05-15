using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_backend.Models.Clauses
{
	/// <summary>
	/// Reprezentacja zdania "A by G releases f"
	/// </summary>
	public class Releases
	{
		public LogicClause InitialCondition { get; }
		public int FluentReleased { get; }
		public int Action { get; }
		public AgentsSet AgentsSet { get; }

		public Releases(LogicClause initialCondition, int fluentReleased, int action, AgentsSet agentsSet)
		{
			InitialCondition = initialCondition;
			FluentReleased = fluentReleased;
			Action = action;
			AgentsSet = agentsSet;
		}
	}
}