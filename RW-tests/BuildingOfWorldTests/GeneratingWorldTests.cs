using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.GraphModels;
using RW_backend.Models.World;

namespace RW_tests.BuildingOfWorldTests
{
	[TestClass]
	public class GeneratingWorldTests
	{
		


		[TestMethod]
		public void GenerateSimpleYaleScenerioWorldFromModelTest()
		{
			// 1. shoot by bob causes !alive if loaded
			// 2. shoot by bob causes !loaded
			// 3. load by bob causes loaded
			var world = new SimpleYaleScenerioWorldGenerator().GenerateYaleWorld(true);
			Console.WriteLine(TestUtilities.WriteOutWorld(world));
			Assert.AreEqual(4, world.States.Count, "wrong number of states");
			Assert.AreEqual(1, world.InitialStates.Count, "wrong number of intial states");
			CheckConnections(world);

		}


		[TestMethod]
		public void PiotrCaseTest()
		{
			Model model = new SimpleYaleScenerioWorldGenerator().GenerateModel();
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			
			var causes1 = new Causes(logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Loaded, FluentSign.Positive),
				logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Negated), YaleScenerio.Shoot, 
				new SimpleYaleScenerioWorldGenerator().SingleAgent(YaleScenerio.Bob));
			model.CausesStatements= new List<Causes>()
			{
				causes1,
			};
			World world = new BackendLogic().CalculateWorld(model);

			Query query =
				new AfterQuery(new ActionAgentsPair[] {new ActionAgentsPair(YaleScenerio.Shoot, YaleScenerio.BobSet)},
					logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Positive), true,
					logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Negated));
			Assert.AreEqual(false, query.Evaluate(world).IsTrue, "wrong result of query");
		}


       

       
        

		private void CheckConnections(World world)
		{
			// TODO: strasznie dużo należałoby sprawdzić...
			var dict = world.Connections[YaleScenerio.Shoot];
			Assert.AreEqual(4, dict.Count, "should be 4 states for shoot");
			dict = world.Connections[YaleScenerio.Load];
			Assert.AreEqual(4, dict.Count, "should be 4 states for load");
		}

		

		
	}
}
