using System.Collections.Generic;
using RW_backend.Logic;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;

namespace RW_tests.BuildingOfWorldTests
{
    static class YaleScenerio
	{
		// agents
		public const int Bob = 0;
		public const int BobSet = 1;
		// fluents
		public const int Alive = 0;
		public const int Loaded = 1;
		// actions
		public const int Shoot = 0;
		public const int Load = 1;
		// states
		public const int NotAliveNotLoaded = 0;
		public const int AliveNotLoaded = 1;
		public const int NotAliveLoaded = 2;
		public const int AliveLoaded = 3;

	}

	class SimpleYaleScenerioWorldGenerator
	{
		public Model GenerateModel(bool initialStates = false)
		{
			// 1. shoot by bob causes !alive if loaded
			// 2. shoot by bob causes !loaded
			// 3. load by bob causes loaded
			Model model = new Model();
			model.ActionsCount = 2;
			model.AgentsCount = 1;
			model.FluentsCount = 2;
			LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
			var causes1 = new Causes(logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Loaded, false),
				logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, true), YaleScenerio.Shoot, SingleAgent(YaleScenerio.Bob));
			var causes2 = new Causes(logicClausesFactory.CreateEmptyLogicClause(),
				logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Loaded, true), YaleScenerio.Shoot, SingleAgent(YaleScenerio.Bob));
			var causes3 = new Causes(logicClausesFactory.CreateEmptyLogicClause(),
				logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Loaded, false),
				YaleScenerio.Load, SingleAgent(YaleScenerio.Bob));
			model.CausesStatements = new List<Causes>() { causes1, causes2, causes3 };
			if (initialStates)
				model.InitiallyStatements = new List<LogicClause>()
			{
				logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Alive, false),
				logicClausesFactory.CreateSingleFluentClause(YaleScenerio.Loaded, false)
			};
			return model;
		}
		public World GenerateYaleWorld(bool initialStates = false)
		{
			var world = new BackendLogic().CalculateWorld(GenerateModel(initialStates));
			return world;
		}

		public AgentsSet SingleAgent(int agentId)
		{
			BitValueOperator bop = new BitValueOperator();
			return new AgentsSet(bop.SetFluent(0, agentId));
		}
	}
}
