using System.Collections.Generic;
using RW_backend.Models.BitSets;

namespace RW_backend.Logic.Queries.Results
{
	public enum Executable
	{
		Never,
		Sometimes,
		Always
	}

	public class ProgramExecutionResult
	{
		public Executable Executable { get; set; }
		public List<State> ReachableStates { get; set; }
	}
}
