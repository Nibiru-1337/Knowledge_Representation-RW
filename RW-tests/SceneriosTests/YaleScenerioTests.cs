using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.GraphModels;
using RW_backend.Models.World;
using RW_tests.BuildingOfWorldTests;

namespace RW_tests.SceneriosTests
{
	[TestClass]
	public class YaleScenerioTests
	{

		[TestMethod]
		public void YaleScenerioBobShootExecutableTest()
		{
			World world = new SimpleYaleScenerioWorldGenerator().GenerateYaleWorld();
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			ExecutableQuery query = new ExecutableQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(YaleScenerio.Shoot, YaleScenerio.BobSet),
			}, logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Positive), false);
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob should be able to shoot");
		}

		[TestMethod]
		public void YaleScenerioBobShootAfterTest()
		{
			World world = new SimpleYaleScenerioWorldGenerator().GenerateYaleWorld();
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			AfterQuery query = new AfterQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(YaleScenerio.Shoot, YaleScenerio.BobSet),
			}, logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Positive), false,
				logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Negated));
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob should be able to kill Fredek if loaded");
		}

		[TestMethod]
		public void YaleScenerioBobShootExecutableAlwaysTest()
		{
			World world = new SimpleYaleScenerioWorldGenerator().GenerateYaleWorld();
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			ExecutableQuery query = new ExecutableQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(YaleScenerio.Shoot, YaleScenerio.BobSet),
			}, logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Positive), true);
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob should be able to shoot");
		}

		[TestMethod]
		public void YaleScenerioBobShootAfterAlwaysTest()
		{
			World world = new SimpleYaleScenerioWorldGenerator().GenerateYaleWorld();
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			AfterQuery query = new AfterQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(YaleScenerio.Shoot, YaleScenerio.BobSet),
			}, logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Positive), true,
				logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Negated));
			Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob should not be able to kill Fredek if ~loaded");
		}

		[TestMethod]
		public void YaleScenerioBobShootImpossibleTest()
		{
			Model model = new SimpleYaleScenerioWorldGenerator().GenerateModel();
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			// dodaj impossible

			model.CausesStatements.Add(new Causes(logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Loaded, false),
				logicClausesFactory.CreateContradictingClause(0), YaleScenerio.Shoot, new SimpleYaleScenerioWorldGenerator().SingleAgent(YaleScenerio.Bob)));
			// jeśli loaded jest prawdziwe, to nie da się użyć SHOOT
			// ale jeśli jest !loaded, to SHOOT nie zabije Fredka
			// zatem Fredek powinien wciąż żyć


			World world = new BackendLogic().CalculateWorld(model);

			AfterQuery query = new AfterQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(YaleScenerio.Shoot, YaleScenerio.BobSet),
			}, logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Positive), false,
				logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, FluentSign.Negated));

			Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob should not be able to kill Fredek anytime");
		}

	}
}
