using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;

namespace RW_tests.ManualTests.SceneriosTests
{
	[TestClass]
	public class ReleasesYaleScenerioTests
	{
		public const int Alive = 0;
		public const int Loaded = 1;
		public const int Happy = 2;

		/*
		World:
		initially happy ^ loaded ^ alive
		SHOOT by Bob causes !loaded ^ !happy
		SHOOT by Bob releases alive
		*/

		/*
		Queries:
		reachable states from initially after SHOOT by Bob:
		!loaded, !happy, !alive
		!loaded, !happy, alive
		*/
		[TestMethod]
		public void ReleasesYaleScenerioTest()
		{
			World world = CreateWorld();
			var query = new ReachableStatesQuery(new ActionAgentsPair[]
			{
				new ActionAgentsPair(0, AgentsSet.CreateFromOneAgent(0).AgentBitSet),
			}, null, false);
			var result = query.RunQuery(world);
			const int n = 2;
			foreach (State state in result.ReachableStates)
			{
				Console.WriteLine(TestUtilities.WriteOutBitSet(state.FluentValues));
			}
			Console.WriteLine("starting: ");
			foreach (State state in world.InitialStates)
			{
				Console.WriteLine(TestUtilities.WriteOutBitSet(state.FluentValues));
			}
			Assert.AreEqual(n, result.ReachableStates.Count, "wrong number of reachable states");
			Assert.AreEqual(false, result.ReachableStates[0].FluentValue(Loaded), "wrong value of fluent " + Loaded);
			Assert.AreEqual(false, result.ReachableStates[1].FluentValue(Loaded), "wrong value of fluent " + Loaded);
			Assert.AreEqual(false, result.ReachableStates[0].FluentValue(Happy), "wrong value of fluent " + Happy);
			Assert.AreEqual(false, result.ReachableStates[1].FluentValue(Happy), "wrong value of fluent " + Happy);
			Assert.IsTrue(
				result.ReachableStates[0].FluentValue(Alive)
				!= result.ReachableStates[1].FluentValue(Alive), "wrong value of Alive");
		}

		private World CreateWorld()
		{
			return new BackendLogic().CalculateWorld(CreateYaleReleasesModel());
		}

		private Model CreateYaleReleasesModel()
		{
			Model model = new Model();
			model.ActionsCount = 1;
			model.AgentsCount = 1;
			model.FluentsCount = 3;
			model.CausesStatements = new List<Causes>()
			{
				new Causes(null, UniformConjunction.CreateFrom(new List<int>() {}, new List<int>() {Loaded, Happy}), 0, 
					AgentsSet.CreateFromOneAgent(0)),
			};
			model.ReleasesStatements = new List<Releases>()
			{
				new Releases(null, Alive, 0, AgentsSet.CreateFromOneAgent(0)),
			};
			model.InitiallyStatements = new List<LogicClause>()
			{
				UniformConjunction.CreateFrom(new List<int>() {Alive, Happy, Loaded}, new List<int>())
			};
			return model;
		}

	}
}
