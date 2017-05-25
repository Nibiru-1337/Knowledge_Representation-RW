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
    class TestWorldGenerator
    {
        readonly LogicClausesFactory _clausesFactory = new LogicClausesFactory();

        public static Model GenerateModel(int fluentsCount, int actionsCount, int agentsCount)
        {
            var model = new Model
            {
                ActionsCount = actionsCount,
                AgentsCount = agentsCount,
                FluentsCount = fluentsCount,
                ActionsNames = new Dictionary<int, string>(),
                AgentsNames = new Dictionary<int, string>(),
                FluentsNames = new Dictionary<int, string>(),
                NoninertialFluents = new HashSet<int>(),
                InitiallyStatements = new List<LogicClause>(),
                AlwaysStatements = new List<LogicClause>(),
                CausesStatements = new List<Causes>(),
                AfterStatements = new List<After>(),
                ReleasesStatements = new List<Releases>()
            };
            return model;
        }

        public AgentsSet SingleAgent(int agentId)
        {
            BitSetOperator bop = new BitSetOperator();
            return new AgentsSet(bop.SetFluent(0, agentId));
        }

        public static AgentsSet AnyAgent()
        {
            return new AgentsSet(0);
        }

        public int AddAction(Model model, string actionName)
        {
            var newActionId = model.ActionsCount;
            model.ActionsNames.Add(newActionId, actionName);
            return newActionId;
        }

        public void AddFluentInvertingAction(Model model, int invertedFluent)
        {
            var actionId = AddAction(model, $"invert{invertedFluent}action");
            model.CausesStatements.Add(new Causes(_clausesFactory.CreateSingleFluentClause(invertedFluent, FluentSign.Positive), _clausesFactory.CreateSingleFluentClause(invertedFluent, FluentSign.Negated), actionId, AnyAgent()));
            model.CausesStatements.Add(new Causes(_clausesFactory.CreateSingleFluentClause(invertedFluent, FluentSign.Negated), _clausesFactory.CreateSingleFluentClause(invertedFluent, FluentSign.Positive), actionId, AnyAgent()));
        }

        public void AddReleasingAction(Model model, int releasedFluent)
        {
            var actionId = AddAction(model, $"release{releasedFluent}action");
            model.ReleasesStatements.Add(new Releases(_clausesFactory.CreateEmptyLogicClause(), releasedFluent, actionId, AnyAgent()));
        }

        public World GenerateSimpleWorld(int fluentsCount, int actionsCount, int agentsCount)
        {
            var world = new BackendLogic().CalculateWorld(GenerateModel(fluentsCount, actionsCount, agentsCount));
            return world;
        }
        public World GenerateWorldWithInvertingAction(int fluentsCount, int actionsCount, int agentsCount = 1, int invertedFluent = 0)
        {
            var model = GenerateModel(fluentsCount, actionsCount, agentsCount);
            AddFluentInvertingAction(model, invertedFluent);
            var world = new BackendLogic().CalculateWorld(model);
            return world;
        }

        public World GenerateWorldWithReleasingAction(int fluentsCount, int actionsCount, int agentsCount = 1, int releasedFluent = 0)
        {
            var model = GenerateModel(fluentsCount, actionsCount, agentsCount);
            AddReleasingAction(model, releasedFluent);
            var world = new BackendLogic().CalculateWorld(model);
            return world;
        }
    }
}
