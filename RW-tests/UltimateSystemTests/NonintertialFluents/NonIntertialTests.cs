﻿using System;
using System.Collections.Generic;
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
        public void CheckForInertialNoReleaseNoOrOneConditionFrom()
        {
            Model model = RaisedHandScenarioGenerator.GenerateWorld(false);
            World world = new BackendLogic().CalculateWorld(model);
            ActionAgentsPair[] program = new ActionAgentsPair[]
            {
	            new ActionAgentsPair(ScenarioConsts.Move, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet)
            };
            //always BobRaised after MOVE by Tom
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            AfterQuery query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always BobRaised after MOVE by Tom");

            //possibly BobRaised after MOVE by Tom
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly BobRaised after MOVE by Tom");


            //possibly TomRaised after MOVE by Tom
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue);

            //always TomRaised after MOVE by Tom
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly TomRaised after MOVE by Tom");

            //possibly TomRaised after MOVE by Tom from ~TomRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, UniformAlternative.CreateFrom(new List<int>(), new List<int>() { ScenarioConsts.TomRaised }), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly TomRaised after MOVE by Tom from ~TomRaised");

            //always TomRaised after MOVE by Tom from ~TomRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, UniformAlternative.CreateFrom(new List<int>(), new List<int>() { ScenarioConsts.TomRaised }), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always TomRaised after MOVE by Tom from ~TomRaised");
        }

        [TestMethod]
        public void CheckForNoninertialNoReleaseNoOrOneConditionFrom()
        {
            Model model = RaisedHandScenarioGenerator.GenerateWorld(true);
            World world = new BackendLogic().CalculateWorld(model);
            ActionAgentsPair[] program = new ActionAgentsPair[]
            {
	            new ActionAgentsPair(ScenarioConsts.Move, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet)
            };
            //always BobRaised after MOVE by Tom
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            AfterQuery query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always BobRaised after MOVE by Tom");

            //possibly BobRaised after MOVE by Tom
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly BobRaised after MOVE by Tom");
            
            //possibly TomRaised after MOVE by Tom from ~TomRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, UniformAlternative.CreateFrom(new List<int>(), new List<int>() { ScenarioConsts.TomRaised }), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly TomRaised after MOVE by Tom from ~TomRaised");

            //always TomRaised after MOVE by Tom from ~TomRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, UniformAlternative.CreateFrom(new List<int>(), new List<int>() { ScenarioConsts.TomRaised }), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always TomRaised after MOVE by Tom from ~TomRaised");
        }

        [TestMethod]
        public void CheckForInertialNoReleaseManyConditionsFrom()
        {
            Model model = RaisedHandScenarioGenerator.GenerateWorld(false);
            World world = new BackendLogic().CalculateWorld(model);
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            ActionAgentsPair[] program = new ActionAgentsPair[]
            {
	            new ActionAgentsPair(ScenarioConsts.Move, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet)
            };

            UniformConjunction fromConds = new UniformConjunction();
            fromConds.AddFluent(ScenarioConsts.TomRaised, FluentSign.Negated);
            fromConds.AddFluent(ScenarioConsts.BobRaised, FluentSign.Negated);

            //always BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised
            AfterQuery query = new AfterQuery(program, fromConds, true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised");

            //possibly BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, fromConds, false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised");

            //possibly Point after MOVE by Tom from ~TomRaised ^ ~BobRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, fromConds, false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Point, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly Point after MOVE by Tom from ~TomRaised ^ ~BobRaised");

            //always Point after MOVE by Tom from ~TomRaised ^ ~BobRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, fromConds, true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Point, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Point after MOVE by Tom from ~TomRaised ^ ~BobRaised");
        }

        [TestMethod]
        public void CheckForNoninertialNoReleaseManyConditionsFrom()
        {
            Model model = RaisedHandScenarioGenerator.GenerateWorld(true);
            World world = new BackendLogic().CalculateWorld(model);
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            ActionAgentsPair[] program = new ActionAgentsPair[]
            {
	            new ActionAgentsPair(ScenarioConsts.Move, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet)
            };

            UniformConjunction fromConds = new UniformConjunction();
            fromConds.AddFluent(ScenarioConsts.TomRaised, FluentSign.Negated);
            fromConds.AddFluent(ScenarioConsts.BobRaised, FluentSign.Negated);

            //always BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised
            AfterQuery query = new AfterQuery(program, fromConds, true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised");

            //possibly BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, fromConds, false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised");

            //possibly Point after MOVE by Tom from ~TomRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, fromConds, false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Point, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly Point after MOVE by Tom from ~TomRaised");

            //always Point after MOVE by Tom from ~TomRaised ^ ~BobRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, fromConds, true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Point, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Point after MOVE by Tom from ~TomRaised ^ ~BobRaised");
        }

        [TestMethod]
        public void CheckForNoninertialRelease()
        {
            Model model = RaisedHandScenarioGenerator.GenerateWorld(true, true);
            World world = new BackendLogic().CalculateWorld(model);
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            ActionAgentsPair[] program = new ActionAgentsPair[]
            {
	            new ActionAgentsPair(ScenarioConsts.Move, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet)
            };

            UniformConjunction fromConds = new UniformConjunction();
            fromConds.AddFluent(ScenarioConsts.TomRaised, FluentSign.Negated);
            fromConds.AddFluent(ScenarioConsts.BobRaised, FluentSign.Negated);

            //always BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised
            AfterQuery query = new AfterQuery(program, fromConds, true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
			Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised");

			//possibly BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised
			// false, because of minimalisation of changes - Point is always (always in that case) treated as change,
			// so when you change one other fluent and Point it is smaller change than changing two other fluents but not Point
			logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, fromConds, false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly BobRaised after MOVE by Tom from ~TomRaised ^ ~BobRaised");

            //possibly Point after MOVE by Tom from ~TomRaised
			// false, because of minimalisation of changes - Point is always (always in that case) treated as change,
			// so when you change one other fluent and Point it is smaller change than changing two other fluents but not Point
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, fromConds, false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Point, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly Point after MOVE by Tom from ~TomRaised");

            //always Point after MOVE by Tom from ~TomRaised ^ ~BobRaised
            logicClausesFactory = new LogicClausesFactory();
            query = new AfterQuery(program, fromConds, true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Point, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Point after MOVE by Tom from ~TomRaised ^ ~BobRaised");
        }
    }
}
