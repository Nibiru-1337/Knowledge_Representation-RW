using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW_backend.Models;

namespace RW_backend.Logic
{
	public class QueryEvaluator
	{
		World World;

		public QueryEvaluator(World world)
		{
			World = world;
		}


		public QueryResult EvaluateQuery(Query query)
		{
			throw new NotImplementedException();
		}
	}
}
