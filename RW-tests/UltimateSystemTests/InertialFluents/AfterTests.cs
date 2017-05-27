using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;
using RW_tests.UltimateSystemTests.IntertialFluents;

namespace RW_tests.UltimateSystemTests.InertialFluents
{
    [TestClass]
    public class AfterTests
    {
        [TestMethod]
        public void NoAddedClauses()
        {
            Model model = BaseWorldGenerator.GenerateWorld();
            World world = new BackendLogic().CalculateWorld(model);
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Tom }));
            program.Add(aap);

            AfterQuery query;

            //always Physics after LEARN by Tom, Bob
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Tom, Bob");

            //possible Physics after LEARN by Tom, Bob
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Tom, Bob");

            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            program.Add(aap);
            //always Physics after LEARN by Bob
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob");

            //possible Physics after LEARN by Bob
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob");


            //always Physics after LEARN by Bob from Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob from Physics");

            //possible Physics after LEARN by Bob from Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob from Physics");


            //always Physics after LEARN by Bob from ~Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob from ~Physics");

            //possible Physics after LEARN by Bob from ~Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob from ~Physics");



            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet);
            program.Add(aap);
            //always Physics after LEARN by Tom from Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Tom from Physics");

            //possible Physics after LEARN by Tom from Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Tom from Physics");


            //always Physics after LEARN by Tom from ~Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Tom from ~Physics");

            //possible Physics after LEARN by Tom from ~Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Tom from ~Physics");

        }
    }
}
