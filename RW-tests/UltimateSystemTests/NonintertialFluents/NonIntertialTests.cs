using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;

namespace RW_tests.UltimateSystemTests.NonintertialFluents
{
    [TestClass]
    public class NonIntertialTests
    {
        [TestMethod]
        public void CheckForInertialNoReleaseNoFrom()
        {
            Model model = BaseWorldGenerator.GenerateWorld(false);
            World world = new BackendLogic().CalculateWorld(model);

            //always BobRaised after MOVE by Tom
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            AfterQuery query = new AfterQuery(new ActionAgentsPair[]
            {
                new ActionAgentsPair(ScenarioConsts.Move, ScenarioConsts.Tom),
            }, 
            new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue);

            //possibly BobRaised after MOVE by Tom
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(new ActionAgentsPair[]
            {
                new ActionAgentsPair(ScenarioConsts.Move, ScenarioConsts.Tom),
            },
            new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue);


            //possibly TomRaised after MOVE by Tom
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(new ActionAgentsPair[]
            {
                new ActionAgentsPair(ScenarioConsts.Move, ScenarioConsts.Tom),
            },
            new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue);

            //always TomRaised after MOVE by Tom
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(new ActionAgentsPair[]
            {
                new ActionAgentsPair(ScenarioConsts.Move, ScenarioConsts.Tom),
            },
            new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue);
        }
    }
}
