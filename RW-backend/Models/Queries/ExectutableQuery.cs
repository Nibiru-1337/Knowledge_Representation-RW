using System;

namespace RW_backend.Models.Queries
{
	class ExectutableQuery : Query
	{
		public override QueryType Type { get { return QueryType.Executable; } }
		public override QueryResult Evaluate(World world)
		{
			throw new NotImplementedException();
		}
	}
}