using System;

namespace RW_backend.Models.Queries
{
	class AfterQuery : Query
	{
		public override QueryType Type => QueryType.After;

		public override QueryResult Evaluate(World world)
		{
			throw new NotImplementedException();
		}
	}
}