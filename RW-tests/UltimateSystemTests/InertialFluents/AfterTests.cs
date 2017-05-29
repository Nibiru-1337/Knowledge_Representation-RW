using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;
using RW_tests.UltimateSystemTests;

namespace RW_tests.UltimateSystemTests.InertialFluents
{
    [TestClass]
    public class AfterTests
    {
        [TestMethod]
        public void NoAddedClauses()
        {
            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            World world = new BackendLogic().CalculateWorld(model);
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap, aap2, aap3, aap4, aap5;
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



            //always Physics after LEARN by Bob
            //LEARN by Tom
            //LEARN by Bob
            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            aap2 = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet);
            program.Add(aap);
            program.Add(aap2);
            query = new AfterQuery(program, logicClausesFactory.CreateEmptyLogicClause(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob (LEARN by Tom, LEARN by Bob)");

            //possible Physics after LEARN by Bob
            //LEARN by Tom
            //LEARN by Bob
            query = new AfterQuery(program, logicClausesFactory.CreateEmptyLogicClause(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob (LEARN by Tom, LEARN by Bob)");


            //always Physics after LEARN by Alice
            //LEARN by Jack 
            //DRINK by Tom
            //LEARN by Bob
            //LEARN by Tom
            //LEARN by Bob
            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Alice).AgentBitSet);
            aap2 = new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Jack }));
            aap3 = new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Tom }));
            aap4 = new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob }));
            aap5 = new ActionAgentsPair(ScenarioConsts.Drink, bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Tom }));
            program.Add(aap);
            program.Add(aap2);
            program.Add(aap3);
            program.Add(aap4);
            program.Add(aap5);
            query = new AfterQuery(program, logicClausesFactory.CreateEmptyLogicClause(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always Physics after LEARN by Alice (LEARN by Jack, DRINK by Tom, LEARN by Bob, LEARN by Tom, LEARN by Bob)");

            //possible Physics after LEARN by Alice
            //LEARN by Jack 
            //DRINK by Tom
            //LEARN by Bob
            //LEARN by Tom
            //LEARN by Bob 
            query = new AfterQuery(program, logicClausesFactory.CreateEmptyLogicClause(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Alice (LEARN by Jack, DRINK by Tom, LEARN by Bob, LEARN by Tom, LEARN by Bob)");
        }

        [TestMethod]
        public void InitiallyNotPhysics()
        {
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            model.InitiallyStatements.Add(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated));
            World world = new BackendLogic().CalculateWorld(model);
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet);
            program.Add(aap);

            AfterQuery query;

            //always Physics after LEARN by Tom
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Tom");

            //possible Physics after LEARN by Tom
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Tom");

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
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob");

            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Alice).AgentBitSet);
            program.Add(aap);
            //always Physics after LEARN by Alice
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always Physics after LEARN by Alice");

            //possible Physics after LEARN by Alice
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Alice");

        }

        [TestMethod]
        public void ExecutableVsAfter()
        {
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            World world = new BackendLogic().CalculateWorld(model);
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet);
            program.Add(aap);
            aap = new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Jack }));
            program.Add(aap);

            ExecutableQuery query1;

            //always executable LEARN by Tom
            //LEARN by Bob, Jack
            query1 = new ExecutableQuery(program, new UniformAlternative(), true);
            Assert.AreEqual(false, query1.Evaluate(world).IsTrue, "always executable LEARN by Tom \n LEARN by Bob, Jack");

            //possible executable LEARN by Tom
            //LEARN by Bob, Jack
            query1 = new ExecutableQuery(program, new UniformAlternative(), false);
            Assert.AreEqual(true, query1.Evaluate(world).IsTrue, "possible executable LEARN by Tom \n LEARN by Bob, Jack");

            AfterQuery query2;
            //always Math after LEARN by Tom
            //LEARN by Bob, Jack
            query2 = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive));
            Assert.AreEqual(true, query2.Evaluate(world).IsTrue, "always Math after LEARN by Tom\nLEARN by Bob, Jack");

            //possible Math after LEARN by Tom
            //LEARN by Bob, Jack
            query2 = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive));
            Assert.AreEqual(true, query2.Evaluate(world).IsTrue, "possible Math after LEARN by Tom\nLEARN by Bob, Jack");

        }

        [TestMethod]
        public void DifferentOrderOfActions()
        {
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            World world = new BackendLogic().CalculateWorld(model);
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            program.Add(aap);
            aap = new ActionAgentsPair(ScenarioConsts.Drink, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            program.Add(aap);

            AfterQuery query;

            //always Math after LEARN by Bob
            //DRINK by Bob
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Math after LEARN by Bob\nDRINK by Bob");

            //possible Math after LEARN by Bob
            //DRINK by Bob
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Math after LEARN by Bob\nDRINK by Bob");

            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            program.Add(aap);
            aap = new ActionAgentsPair(ScenarioConsts.Drink, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet);
            program.Add(aap);

            //always Math after LEARN by Bob
            //DRINK by Tom
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Math after LEARN by Bob\nDRINK by Tom");

            //possible Math after LEARN by Bob
            //DRINK by Tom
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possible Math after LEARN by Bob\nDRINK by Tom");


            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Drink, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet);
            program.Add(aap);
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            program.Add(aap);

            //always DRINK by Tom 
            //Math after LEARN by Bob
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always Math after DRINK by Tom\nLEARN by Bob");

            //possible DRINK by Tom 
            //Math after LEARN by Bob
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Math after DRINK by Tom\nLEARN by Bob");

        }

        [TestMethod]
        public void AfterClauseVsQueryTwoActionsTwoAgents()
        {
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            program.Add(aap);
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet);
            program.Add(aap);

            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            After after = new After(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), program, true);
            model.AfterStatements.Add(after);
            World world = new BackendLogic().CalculateWorld(model);


            AfterQuery query;

            //always Physics after LEARN by Bob
            //LEARN by Tom
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob \n LEARN by Tom");

            //possible Physics after LEARN by Bob
            //LEARN by Tom
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob \n LEARN by Tom");

            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet);
            program.Add(aap);
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            program.Add(aap);

            //always Physics after LEARN by Tom
            //LEARN by Bob
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Tom \n LEARN by Bob");

            //possible Physics after LEARN by Tom
            //LEARN by Bob
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Tom \n LEARN by Bob");

        }

        [TestMethod]
        public void AfterClauseVsQueryObservable()
        {
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();

            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            program.Add(aap);

            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            After after =
                new After(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), program, false);
            model.AfterStatements.Add(after);
            World world = new BackendLogic().CalculateWorld(model);


            AfterQuery query;

            //always Physics after LEARN by Bob from ~Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob from ~Physics");

            //possible  Physics after LEARN by Bob from ~Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob from ~Physics");

        }

        [TestMethod]
        public void AfterClauseVsQueryOneAgent()
        {
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();

            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob).AgentBitSet);
            program.Add(aap);

            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            After after = new After(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), program, true);
            model.AfterStatements.Add(after);
            World world = new BackendLogic().CalculateWorld(model);


            AfterQuery query;

            //always Physics after LEARN by Bob
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob");

            //possible Physics after LEARN by Bob
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob");

            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int>() {ScenarioConsts.Bob, ScenarioConsts.Tom}));
            program.Add(aap);

            //always Physics after LEARN by Bob, Tom
            query = new AfterQuery(program, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob, Tom");

            //possible Physics after LEARN by Bob, Tom
            query = new AfterQuery(program, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob, Tom");


            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Jack }));
            program.Add(aap);


            //always Physics after LEARN by Bob, Jack from ~Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob, Jack from ~Physics");

            //possible Physics after LEARN by Bob, Jack from ~Physics
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob, Jack from ~Physics");



            //always Physics after LEARN by Bob, Jack from ~HasToy
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Positive));
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always Physics after LEARN by Bob, Jack from ~HasToy");

            //possible Physics after LEARN by Bob, Jack from ~HasToy
            query = new AfterQuery(program, logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Positive));
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possible Physics after LEARN by Bob, Jack from ~HasToy");

        }

        [TestMethod]
        public void Case11ATestPossiblyNotAlways()
        {
            var logicClausesFactory = new LogicClausesFactory();

            var bitSetFactory = new BitSetFactory();
            var afterProgram = new List<ActionAgentsPair> { new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Bob, ScenarioConsts.Tom })) };
            var queryProgram = new List<ActionAgentsPair>
            {
                new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> {ScenarioConsts.Tom})),
                new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> {ScenarioConsts.Bob}))
            };
            var model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            var after = new After(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), afterProgram, true);
            model.AfterStatements.Add(after);
            var world = new BackendLogic().CalculateWorld(model);


            //possibly Physics after LEARN by Tom, LEARN by Bob
            var possiblyQuery = new AfterQuery(queryProgram, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, possiblyQuery.Evaluate(world).IsTrue, "possibly Physics after LEARN by Tom, LEARN by Bob");

            //always Physics after LEARN by Tom, LEARN by Bob
            var alwaysQuery = new AfterQuery(queryProgram, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(false, alwaysQuery.Evaluate(world).IsTrue, "always Physics after LEARN by Tom, LEARN by Bob");
        }

        [TestMethod]
        public void Case11BTestPossiblyAlways()
        {
            var logicClausesFactory = new LogicClausesFactory();

            var bitSetFactory = new BitSetFactory();
            var afterProgram = new List<ActionAgentsPair> { new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Bob, ScenarioConsts.Tom })) };
            var queryProgram = new List<ActionAgentsPair> { new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Tom, ScenarioConsts.Bob })) };
            var model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            var after = new After(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), afterProgram, true);
            model.AfterStatements.Add(after);
            var world = new BackendLogic().CalculateWorld(model);


            //possibly Physics after LEARN by Tom,Bob
            var possiblyQuery = new AfterQuery(queryProgram, new UniformAlternative(), false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, possiblyQuery.Evaluate(world).IsTrue, "possibly Physics after LEARN by Tom,Bob");

            //always Physics after LEARN by Tom,Bob
            var alwaysQuery = new AfterQuery(queryProgram, new UniformAlternative(), true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, alwaysQuery.Evaluate(world).IsTrue, "always Physics after LEARN by Tom,Bob");
        }

        [TestMethod]
        public void Case11CTestPossiblyAlwaysWithJack()
        {
            var logicClausesFactory = new LogicClausesFactory();

            var bitSetFactory = new BitSetFactory();
            var afterProgram = new List<ActionAgentsPair> { new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Bob, ScenarioConsts.Tom })) };
            var queryProgram = new List<ActionAgentsPair> { new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Tom, ScenarioConsts.Bob, ScenarioConsts.Jack })) };
            var model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            var after = new After(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), afterProgram, true);
            model.AfterStatements.Add(after);
            var world = new BackendLogic().CalculateWorld(model);

            var queryFrom = logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Positive);
            //possibly Physics after LEARN by Tom,Bob,Jack from HasToy
            var possiblyQuery = new AfterQuery(queryProgram, queryFrom, false,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, possiblyQuery.Evaluate(world).IsTrue, "possibly Physics after LEARN by Tom,Bob,Jack from HasToy");

            //always Physics after LEARN by Tom,Bob,Jack from HasToy
            var alwaysQuery = new AfterQuery(queryProgram, queryFrom, true,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive));
            Assert.AreEqual(true, alwaysQuery.Evaluate(world).IsTrue, "always Physics after LEARN by Tom,Bob,Jack from HasToy");
        }

        [TestMethod]
        public void Case16TestInitiallyHatToyQueryFromNotHasToy()
        {
            var logicClausesFactory = new LogicClausesFactory();

            var bitSetFactory = new BitSetFactory();
            var queryProgram = new List<ActionAgentsPair> { new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Tom, ScenarioConsts.Jack })) };
            var model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            var initiallyHasToy = logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Positive);
            model.InitiallyStatements.Add(initiallyHasToy);
            var world = new BackendLogic().CalculateWorld(model);

            var fromNotHasToy = logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Negated);
            var executableQuery = new ExecutableQuery(queryProgram, fromNotHasToy, false);
            Assert.AreEqual(false, executableQuery.Evaluate(world).IsTrue, "executable LEARN by Tom,Jack from !HasToy");

            var alwaysExecutableQuery = new ExecutableQuery(queryProgram, fromNotHasToy, true);
            Assert.AreEqual(false, alwaysExecutableQuery.Evaluate(world).IsTrue, "always executable LEARN by Tom,Jack from !HasToy");
        }

        [TestMethod]
        public void Case17ATestAlways()
        {
            var logicClausesFactory = new LogicClausesFactory();

            var bitSetFactory = new BitSetFactory();
            var afterProgram = new List<ActionAgentsPair> { new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Tom })) };
            var queryProgram = new List<ActionAgentsPair> { new ActionAgentsPair(ScenarioConsts.Learn, bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Tom })) };
            var model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            var after = new After(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive), afterProgram, true);
            model.AfterStatements.Add(after);
            var alwaysNotPhysics = logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated);
            model.AlwaysStatements.Add(alwaysNotPhysics);
            var world = new BackendLogic().CalculateWorld(model);

            var executableQuery = new ExecutableQuery(queryProgram, new UniformAlternative(), false);
            Assert.AreEqual(false, executableQuery.Evaluate(world).IsTrue, "executable LEARN by Tom");

            var alwaysExecutableQuery = new ExecutableQuery(queryProgram, new UniformAlternative(), true);
            Assert.AreEqual(false, alwaysExecutableQuery.Evaluate(world).IsTrue, "always executable LEARN by Tom");
        }

        [TestMethod]
        public void Case17BTestAlwaysInitially()
        {
            var logicClausesFactory = new LogicClausesFactory();

            var bitSetFactory = new BitSetFactory();
            var model = PatriciaExamSessionScenratioGenerator.GenerateModel();

            var initiallyPhysics = logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Positive);
            model.InitiallyStatements.Add(initiallyPhysics);
            var alwaysNotPhysics = logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Physics, FluentSign.Negated);
            model.AlwaysStatements.Add(alwaysNotPhysics);
            var world = new BackendLogic().CalculateWorld(model);

            Assert.IsTrue(world.Inconsistent, "world inconsistent - contradicting initially and always");
        }
    }
}
