using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;

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



		public void UltimateYaleTest()
		{
			World world = new BackendLogic().CalculateWorld(CreateModel());





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
				}
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
				new Causes(factory.CreateSingleFluentClause(JohnLoaded, false), factory.CreateSingleFluentClause(JohnLoaded, true), 
					Shoot, new AgentsSet(new BitSetFactory().CreateFromOneElement(John))),
				// releases by Tom w Releases
				// impossible shoot with Jack
				Causes.CreateImpossible(factory.CreateEmptyLogicClause(), Shoot, 
					new AgentsSet(new BitSetFactory().CreateFromOneElement(Jack))),
			};
		}

		private Causes UnloadCauses(LogicClausesFactory factory, int agent, int fluent)
		{
			BitValueOperator bop = new BitValueOperator();
			return new Causes(factory.CreateSingleFluentClause(fluent, false), 
				factory.CreateSingleFluentClause(fluent, true), Shoot, new AgentsSet(bop.SetFluent(0, agent)));
		}



	}
}
