using System;

namespace RW_backend.Models.Queries
{
	class EngagedQuery : Query
	{
		public override QueryType Type { get { return QueryType.Engaged; } }
		public override QueryResult Evaluate(World world)
		{
			throw new NotImplementedException();
		}
	}
}