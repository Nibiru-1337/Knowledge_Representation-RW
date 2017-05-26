using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
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

        [TestMethod]
        public void ActionWithReleasesWithoutCausesTest()
        {
            const int fluentsCount = 1;
            const int actionsCount = 1, action = 0;
            State state0 = new State(0), state1 = new State(1);
            var world = new TestWorldGenerator().GenerateWorldWithReleasingAction(fluentsCount, actionsCount);

            Assert.IsNotNull(world, "world is null");
            Assert.AreEqual(actionsCount, world.ActionIds.Count, "wrong action count");
            //Assert.IsTrue(world.Connections[action][state0].Any(asc=>asc.Edges.Contains(state1)), "no state1 in reachable states");
            //Assert.IsTrue(world.Connections[action][state1].Any(asc=>asc.Edges.Contains(state0)), "no state0 in reachable states");
			
			// state0:
			ReachableStatesQuery query0 = GetQueryForReleasesWithoutCausesTest(0, 0, 0, true);
			// state1:
			ReachableStatesQuery query1 = GetQueryForReleasesWithoutCausesTest(0, 0, 0, false);
			CheckResultFromQueryInReleasesWithoutCausesTest(query0, world, state0, state1, action);
			CheckResultFromQueryInReleasesWithoutCausesTest(query1, world, state0, state1, action);

		}

		private void CheckResultFromQueryInReleasesWithoutCausesTest(ReachableStatesQuery query,
			World world, State state0, State state1, int action)
		{
			var result = query.GetDetailsFromExecution(world);
			Assert.IsTrue(result.ReachableStates.Any(asc => Equals(asc, state1)), "no state1 in reachable states");
			Assert.IsTrue(result.ReachableStates.Any(asc => Equals(asc, state0)), "no state0 in reachable states");
		}

		private ReachableStatesQuery GetQueryForReleasesWithoutCausesTest(int action, int agent, int fluent, bool ifState0)
		{
			List<int> positive = null, negated = null;
			if (ifState0)
				positive = new List<int>() {fluent};
			else negated = new List<int>() {fluent};
			return new ReachableStatesQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(action, AgentsSet.CreateFromOneAgent(agent).AgentBitSet)
			}, UniformConjunction.CreateFrom(positive, negated), false); // czyli ze stanu state1
		}

        [TestMethod]
        public void ActionInvertingFluentTest()
        {
            const int fluentsCount = 1;
            const int actionsCount = 1, action = 0;
            State state0 = new State(0), state1 = new State(1);
            var world = new TestWorldGenerator().GenerateWorldWithInvertingAction(fluentsCount, actionsCount);

            Assert.IsNotNull(world);
            Assert.AreEqual(actionsCount, world.ActionIds.Count);
            Assert.IsTrue(world.Connections[action][state0].Any(asc=>asc.Edges.Contains(state1)));
            Assert.IsTrue(world.Connections[action][state1].Any(asc=>asc.Edges.Contains(state0)));
        }
	}
}
