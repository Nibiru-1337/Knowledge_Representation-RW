using System;

namespace RW_backend.Models.Queries
{
	class AfterQuery : Query
	{
		public override QueryType Type { get { return QueryType.After; } }
		public override QueryResult Evaluate(World world)
		{
			throw new NotImplementedException();
		}
	}
}