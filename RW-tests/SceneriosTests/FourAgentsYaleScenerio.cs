using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.GraphModels;
using RW_backend.Models.World;
using RW_tests.BuildingOfWorldTests;

namespace RW_tests.SceneriosTests
{
	[TestClass]
	public class FourAgentsYaleScenerio
	{
		// agents
		const int John = 0;
		const int Tom = 1;
		const int Bob = 2;
		const int Jack = 3;
		// actions
		const int Shoot = 0;
		// fluents
		const int Alive = 0;
		const int JohnLoaded = 1;
		const int TomLoaded = 2;
		const int BobLoaded = 3;

		readonly List<string> FluentsNames = new List<string>() {"Alive", "JohnLoaded", "TomLoaded", "BobLoaded"};
		readonly List<string> AgentsNames = new List<string>() {"John", "Tom", "Bob", "Jack"};
		readonly List<string> ActionsNames = new List<string>() {"SHOOT"};

		[TestMethod]
		public void YaleScenerioConsistentTest()
		{
			World world = new BackendLogic().CalculateWorld(CreateModel());
			Console.WriteLine(TestUtilities.WriteOutWorld(world));
			Assert.AreEqual(false, world.Inconsistent, "world should be consistent!");
		}

		[TestMethod]
		public void ExecutableShootByTomJohnBob()
		{
			World world = CreateWorld();
			//Console.WriteLine(TestUtilities.WriteOutWorld(world));
			ExecutableQuery query = new ExecutableQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(Shoot, GetAgentsSet(new List<int>() {Bob, Tom, John}).AgentSet),
			}, null, false);
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "wrong result of query");
		}

		[TestMethod]
		public void PossiblyDeadAfterShootByTomBob()
		{
			World world = CreateWorld();
			//Console.WriteLine(TestUtilities.WriteOutWorld(world));
			AfterQuery query = new AfterQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(Shoot, GetAgentsSet(new List<int>() {Bob, Tom}).AgentSet),
			}, null, false, new LogicClausesFactory().CreateSingleFluentClause(Alive, true));
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "wrong result of query");

		}

		[TestMethod]
		public void NotNecesseryDeadAfterShootByTomBob()
		{
			World world = CreateWorld();
			//Console.WriteLine(TestUtilities.WriteOutWorld(world));
			AfterQuery query = new AfterQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(Shoot, GetAgentsSet(new List<int>() {Bob, Tom}).AgentSet),
			}, null, true, new LogicClausesFactory().CreateSingleFluentClause(Alive, true));
			Assert.AreEqual(false, query.Evaluate(world).IsTrue, "wrong result of query");
		}

		[TestMethod]
		public void NecesseryDeadAfterShootByTomBobJohn()
		{
			World world = CreateWorld();
			Console.WriteLine(TestUtilities.WriteOutWorldFomInitiallyOnly(world, FluentsNames, AgentsNames, ActionsNames));
			AfterQuery query = new AfterQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(Shoot, GetAgentsSet(new List<int>() {Bob, Tom, John}).AgentSet),
			}, null, true, new LogicClausesFactory().CreateSingleFluentClause(Alive, FluentSign.Negated));
			var result = query.Evaluate(world);
			//Console.WriteLine(string.Join(",", result.StatePath));
			Assert.AreEqual(true, result.IsTrue, "wrong result of query");
		}

		[TestMethod]
		public void NecesseryNotDeadAfterShootByTomBobJohnJack()
		{
			World world = CreateWorld();
			Console.WriteLine(TestUtilities.WriteOutWorldFomInitiallyOnly(world, FluentsNames, AgentsNames, ActionsNames));
			AfterQuery query = new AfterQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(Shoot, GetAgentsSet(new List<int>() {Bob, Tom, John, Jack}).AgentSet),
			}, null, true, new LogicClausesFactory().CreateSingleFluentClause(Alive, FluentSign.Positive));
			var result = query.Evaluate(world);
			//Console.WriteLine(string.Join(",", result.StatePath));
			Assert.AreEqual(true, result.IsTrue, "wrong result of query");
		}

		[TestMethod]
		public void EngagedBobInShootByTomBobJohn()
		{
			World world = CreateWorld();
			//Console.WriteLine(TestUtilities.WriteOutWorld(world));
			EngagedQuery query = new EngagedQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(Shoot, GetAgentsSet(new List<int>() {Bob, Tom, John}).AgentSet),
			}, null, true, GetAgentsSet(new List<int>() {Bob}));
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "wrong result of query");
		}

		private AgentsSet GetAgentsSet(List<int> agentsSet)
		{
			BitSetOperator bop = new BitSetOperator();
			int set = agentsSet.Aggregate(0, (current, agent) => bop.SetFluent(current, agent));
			return new AgentsSet(set);
		}

		private AgentsSet GetAgentsSet(bool JohnInSet, bool TomInSet, bool BobInSet, bool JackInSet)
		{
			BitSetOperator bop = new BitSetOperator();
			int set = 0;
			if (JohnInSet) set = bop.SetFluent(set, John);
			if (TomInSet) set = bop.SetFluent(set, Tom);
			if (BobInSet) set = bop.SetFluent(set, Bob);
			if (JackInSet) set = bop.SetFluent(set, Jack);
			return new AgentsSet(set);
		}

		private World CreateWorld()
		{
			return new BackendLogic().CalculateWorld(CreateModel());
		}

		private Model CreateModel()
		{
			Model model = new Model
			{
				CausesStatements = CreateCauses(),
				ReleasesStatements = new List<Releases>()
				{
					// Shoot by Tom releases Alive if Alive ^ TomLoaded
					new Releases(
						UniformConjunction.CreateFrom(new List<int>() {Alive, TomLoaded}, new List<int>()),
						Alive, Shoot,
						new AgentsSet(new BitSetFactory().CreateFromOneElement(Tom)))
				},
				InitiallyStatements = new List<LogicClause>()
				{
					UniformConjunction.CreateFrom(
						new List<int>() {Alive, TomLoaded, BobLoaded, JohnLoaded}, new List<int>())
				},
				ActionsCount = 1,
				FluentsCount = 4,
				AgentsCount = 4,
			};
			
			return model;
		}


		private List<Causes> CreateCauses()
		{
			LogicClausesFactory factory = new LogicClausesFactory();
			return new List<Causes>()
			{
				// Shoot by John causes ~JohnLoaded if JohnLoaded itd.
				UnloadCauses(factory, John, JohnLoaded),
				UnloadCauses(factory, Bob, BobLoaded),
				UnloadCauses(factory, Tom, TomLoaded),
				// Shoot by John causes ~Alive if JohnLoaded
				new Causes(factory.CreateSingleFluentClause(JohnLoaded, FluentSign.Positive), 
				factory.CreateSingleFluentClause(Alive, FluentSign.Negated), 
					Shoot, GetAgentsSet(new List<int>() {John})),
				// releases by Tom w Releases
				// impossible shoot with Jack
				Causes.CreateImpossible(factory.CreateEmptyLogicClause(), Shoot,
					GetAgentsSet(new List<int>() {Jack})),
			};
		}

		private Causes UnloadCauses(LogicClausesFactory factory, int agent, int fluent)
		{
			BitSetOperator bop = new BitSetOperator();
			return new Causes(factory.CreateSingleFluentClause(fluent, false), 
				factory.CreateSingleFluentClause(fluent, true), Shoot, new AgentsSet(bop.SetFluent(0, agent)));
		}



	}
}
