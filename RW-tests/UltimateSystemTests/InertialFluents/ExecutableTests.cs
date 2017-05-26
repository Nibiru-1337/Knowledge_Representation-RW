using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;
using RW_tests.UltimateSystemTests.IntertialFluents;

namespace RW_tests.UltimateSystemTests.InertialFluents
{
    [TestClass]
    public class ExecutableTests
    {
        [TestMethod]
        public void NoAddedClauses()
        {
            Model model = BaseWorldGenerator.GenerateWorld();
            World world = new BackendLogic().CalculateWorld(model);
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();// = new ActionAgentsPair[] { new ActionAgentsPair(ScenarioConsts.Move, ScenarioConsts.Tom) };


            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn, 
                bitSetFactory.CreateBitSetValueFrom(new List<int>() {ScenarioConsts.Bob, ScenarioConsts.Jack}));
            program.Add(aap);
            //always executable LEARN by Bob, Jack from ~HasToy
            ExecutableQuery query = new ExecutableQuery(program, 
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Negated), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always executable LEARN by Bob, Jack from ~HasToy");
            
            //possibly executable LEARN by Bob, Jack from ~HasToy
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Negated), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly executable LEARN by Bob, Jack from ~HasToy");

            //always executable LEARN by Bob, Jack
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always executable LEARN by Bob, Jack");

            //possibly executable LEARN by Bob, Jack
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly executable LEARN by Bob, Jack");

        }
    }
}
