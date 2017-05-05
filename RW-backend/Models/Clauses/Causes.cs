using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_backend.Models.Clauses
{
	/// <summary>
	/// Reprezentacja zdania "A by G causes a if pi"
	/// </summary>
	public class Causes
	{
		public LogicClause Condition { get; }
		public LogicClause Effect { get; }
		public int Action { get; }
		public AgentsSet AgentsSet { get; }

		public Causes(LogicClause condition, LogicClause effect, int action, AgentsSet agentsSet)
		{
			Condition = condition;
			Effect = effect;
			Action = action;
			AgentsSet = agentsSet;
		}

		


	}
}