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
		// agents
		const int Bob = 0;
		const int BobSet = 1;
		// fluents
		const int Alive = 0;
		const int Loaded = 1;
		// actions
		const int Shoot = 0;
		const int Load = 1;
		// states
		const int NotAliveNotLoaded = 0;
		const int AliveNotLoaded = 1;
		const int NotAliveLoaded = 2;
		const int AliveLoaded = 3;


		[TestMethod]
		public void GenerateSimpleYaleScenerioWorldFromModelTest()
		{
			// 1. shoot by bob causes !alive if loaded
			// 2. shoot by bob causes !loaded
			// 3. load by bob causes loaded
			var world = GenerateYaleWorld(true);
			Console.WriteLine(WriteOutWorld(world));
			Assert.AreEqual(4, world.States.Count, "wrong number of states");
			Assert.AreEqual(1, world.InitialStates.Count, "wrong number of intial states");
			CheckConnections(world);

		}

		[TestMethod]
		public void YaleScenerioTest()
		{
			World world = GenerateYaleWorld();
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			ExecutableQuery query = new ExecutableQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(Shoot, BobSet),
			}, false, logicClausesFactory.CreateSingleFluentClause(Alive, false));
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "query should pass");

		}

		World GenerateYaleWorld(bool initialStates = false)
		{
			// 1. shoot by bob causes !alive if loaded
			// 2. shoot by bob causes !loaded
			// 3. load by bob causes loaded
			Model model = new Model();
			model.ActionsCount = 2;
			model.AgentsCount = 1;
			model.FluentsCount = 2;
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			var causes1 = new Causes(logicClausesFactory.CreateSingleFluentClause(Loaded, false),
				logicClausesFactory.CreateSingleFluentClause(Alive, true), Shoot, SingleAgent(Bob));
			var causes2 = new Causes(logicClausesFactory.CreateEmptyLogicClause(),
				logicClausesFactory.CreateSingleFluentClause(Loaded, true), Shoot, SingleAgent(Bob));
			var causes3 = new Causes(logicClausesFactory.CreateEmptyLogicClause(),
				logicClausesFactory.CreateSingleFluentClause(Loaded, false),
				Load, SingleAgent(Bob));
			model.CausesStatements = new List<Causes>() { causes1, causes2, causes3 };
			if(initialStates)
				model.InitiallyStatements = new List<LogicClause>()
			{
				logicClausesFactory.CreateSingleFluentClause(Alive, false),
				logicClausesFactory.CreateSingleFluentClause(Loaded, false)
			};

			var world = new BackendLogic().CalculateWorld(model);
			return world;
		}

		private void CheckConnections(World world)
		{
			// TODO: strasznie dużo należałoby sprawdzić...
			var dict = world.Connections[Shoot];
			Assert.AreEqual(4, dict.Count, "should be 4 states for shoot");
			dict = world.Connections[Load];
			Assert.AreEqual(4, dict.Count, "should be 4 states for load");
		}

		private string WriteOutWorld(World world)
		{
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<int, Dictionary<State, IList<AgentSetChecker>>> connection in world.Connections)
			{
				sb.Append("for action = " + connection.Key).AppendLine();
				foreach (KeyValuePair<State, IList<AgentSetChecker>> pair in connection.Value)
				{
					sb.Append("\tfor state = ").Append(pair.Key).AppendLine();
					foreach (AgentSetChecker setChecker in pair.Value)
					{
						sb.Append("\t\tfor agents set = ").Append(setChecker.AgentsSet).AppendLine();
						foreach (State state in setChecker.Edges)
						{
							sb.Append("\t\t\tcan go to state ").Append(state).AppendLine();
						}
					}

				}


			}


			return sb.ToString();

		}

		private AgentsSet SingleAgent(int agentId)
		{
			BitValueOperator bop = new BitValueOperator();
			return new AgentsSet(bop.SetFluent(0, agentId));
		}
	}
}
