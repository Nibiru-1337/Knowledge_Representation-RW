using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_backend.Models.Clauses
{
	/// <summary>
	/// Reprezentacja zdania "A by G causes a if pi"
	/// </summary>
	public class Causes
	{
		public LogicClause InitialCondition { get; }
		public LogicClause Effect { get; }
		public int Action { get; }
		public AgentsSet AgentsSet { get; }

		public Causes(LogicClause initialCondition, LogicClause effect, int action, AgentsSet agentsSet)
		{
			InitialCondition = initialCondition;
			Effect = effect;
			Action = action;
			AgentsSet = agentsSet;
		}

		public override string ToString()
		{
			return "condition = " + InitialCondition + ", effect = " + Effect + ", Action = "
					+ Action + ", Agents = " + AgentsSet;
		}
	}
}