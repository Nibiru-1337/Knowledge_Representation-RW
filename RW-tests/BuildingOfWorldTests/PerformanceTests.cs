using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.Factories;
using RW_backend.Models.GraphModels;
using RW_backend.Models.World;

namespace RW_tests.BuildingOfWorldTests
{
	[TestClass]
	public class PerformanceTests
	{
		[TestMethod]
		public void YaleScenerioBobShootExecutableAlways10FluentsTest()
		{
			var world = YaleWorldWithMoreFluents(10);
			var query = YaleExecutableQuery();
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob should be able to shoot");
		}

		[TestMethod]
		public void YaleScenerioIncreasingFluentsCountTest()
		{
			const int MinimumProblemSize = 1;
			const int DesiredProblemSize = 20;
			int fluentsCount;
			for (fluentsCount = 11; fluentsCount <= DesiredProblemSize; fluentsCount++)
			{
				try
				{
					var world = YaleWorldWithMoreFluents(fluentsCount);
					var query = YaleExecutableQuery();
					Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob should be able to shoot");
				}
				catch (OutOfMemoryException) { break; }
				Console.WriteLine("for " + fluentsCount + " is ok");
			}
			if (fluentsCount > DesiredProblemSize)
			{
				Console.WriteLine("Your RAM size rocks! (or the algorithm was improved)");
				return;
			}
			Console.WriteLine("Your RAM failed at {0} fluents", fluentsCount);
			Assert.IsTrue(fluentsCount > MinimumProblemSize, "Buy more RAM");
		}


		private static ExecutableQuery YaleExecutableQuery()
		{
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			ExecutableQuery query = new ExecutableQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(YaleScenerio.Shoot, YaleScenerio.BobSet),
			}, logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, false), true);
			return query;
		}

		private World YaleWorldWithMoreFluents(int fluentsCount)
		{
			Model model = new SimpleYaleScenerioWorldGenerator().GenerateModel();
			model.FluentsCount = fluentsCount;
			World world = new BackendLogic().CalculateWorld(model);
			return world;
		}


	}
}
