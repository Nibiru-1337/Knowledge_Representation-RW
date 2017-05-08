using System.Collections.Generic;
using RW_backend.Models.BitSets;

namespace RW_backend.Logic.Queries.Results
{
	public class QueryResult
	{
		public bool IsTrue;
		public List<State> SuccessfulPath;
		public List<State> WrongPath;
		public List<State> StatePath => SuccessfulPath;
	}
}