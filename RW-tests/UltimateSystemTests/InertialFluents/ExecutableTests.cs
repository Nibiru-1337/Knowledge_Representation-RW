using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;

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
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Negated), false);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly executable LEARN by Bob, Jack from ~HasToy");

            //always executable LEARN by Bob, Jack
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always executable LEARN by Bob, Jack");

            //possibly executable LEARN by Bob, Jack
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), false);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly executable LEARN by Bob, Jack");

            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Jack }));
            program= new List<ActionAgentsPair>() {aap};

            //always executable LEARN by Alice, Bob, Jack
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always executable LEARN by Alice, Bob, Jack");

            //possibly executable LEARN by Alice, Bob, Jack
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), false);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly executable LEARN by Alice, Bob, Jack");


            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Alice }));
            program.Add(aap);

            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Jack }));
            program.Add(aap);
            
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob }));
            program.Add(aap);

            //always executable LEARN by Alice
            //LEARN by Jack
            //LEARN by Bob
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always executable LEARN by Alice\nLEARN by Jack\nLEARN by Bob");

            //possibly executable LEARN by Alice
            //LEARN by Jack
            //LEARN by Bob
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), false);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly executable LEARN by Alice\nLEARN by Jack\nLEARN by Bob");



            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob }));
            program.Add(aap);

            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Jack }));
            program.Add(aap);

            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Alice }));
            program.Add(aap);

            //always executable LEARN by Bob
            //LEARN by Jack
            //LEARN by Alice
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always executable LEARN by Bob\nLEARN by Jack\nLEARN by Alice");

            //possibly executable LEARN by Bob
            //LEARN by Jack
            //LEARN by Alice
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), false);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly executable LEARN by Bob\nLEARN by Jack\nLEARN by Alice");


            //always executable LEARN by Bob from ~HasToy
            //LEARN by Jack
            //LEARN by Alice
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Negated), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always executable LEARN by Bob from ~HasToy\nLEARN by Jack\nLEARN by Alice");

            //possibly executable LEARN by Bob from ~HasToy
            //LEARN by Jack
            //LEARN by Alice
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Negated), false);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly executable LEARN by Bob from ~HasToy\nLEARN by Jack\nLEARN by Alice");
        }

        [TestMethod]
        public void AlwaysNotHasToy()
        {
            Model model = BaseWorldGenerator.GenerateWorld();
            UniformConjunction uc = UniformConjunction.CreateFrom(new List<int>(), new List<int>() { ScenarioConsts.HasToy });
            model.AlwaysStatements.Add(uc);
            
            World world = new BackendLogic().CalculateWorld(model);
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();


            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Jack }));
            program.Add(aap);

            //always executable LEARN by Bob, Jack
            ExecutableQuery query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always executable LEARN by Bob, Jack");

            //possibly executable LEARN by Bob, Jack
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), false);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly executable LEARN by Bob, Jack");

            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Alice).AgentBitSet);
            program.Add(aap);
            //possibly executable LEARN by Alice
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), false);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly executable LEARN by Alice");

            //always executable LEARN by Alice
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always executable LEARN by Alice");

            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Tom }));
            program.Add(aap);

            //possibly executable LEARN by Tom, Bob
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), false);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly executable LEARN by Tom, Bob");

            //possibly executable LEARN by Tom, Bob
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always executable LEARN by Tom, Bob");
            
        }

        [TestMethod]
        public void InitiallyHasToy()
        {
            Model model = BaseWorldGenerator.GenerateWorld();
            UniformConjunction uc = UniformConjunction.CreateFrom(new List<int>() { ScenarioConsts.HasToy }, new List<int>() );
            model.InitiallyStatements.Add(uc);

            World world = new BackendLogic().CalculateWorld(model);
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();


            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Tom, ScenarioConsts.Jack }));
            program.Add(aap);


			//possibly executable LEARN by Tom, Jack
			ExecutableQuery query = new ExecutableQuery(program,
				logicClausesFactory.CreateEmptyLogicClause(), false);
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly executable LEARN by Tom, Jack");

			//always executable LEARN by Tom, Jack
			query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
			Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always executable LEARN by Tom, Jack");
            
        }

        [TestMethod]
        public void ContradictingResults()
        {

            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            Model model = BaseWorldGenerator.GenerateWorld();
            Causes cause = new Causes(new UniformAlternative(), logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Negated),
                ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Alice));
            model.CausesStatements.Add(cause);

            World world = new BackendLogic().CalculateWorld(model);
            BitSetFactory bitSetFactory = new BitSetFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();


            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Tom, ScenarioConsts.Alice }));
            program.Add(aap);

            //always executable LEARN by Alice, Tom
            ExecutableQuery query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "always executable LEARN by Alice, Tom");

            //possibly executable LEARN by Alice, Tom
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), false);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "possibly executable LEARN by Alice, Tom");

            program = new List<ActionAgentsPair>();
            aap = new ActionAgentsPair(ScenarioConsts.Learn,AgentsSet.CreateFromOneAgent(ScenarioConsts.Alice).AgentBitSet);
            program.Add(aap);

            aap = new ActionAgentsPair(ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom).AgentBitSet);
            program.Add(aap);


            //always executable LEARN by Alice
            //LEARN by Tom
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), true);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "always executable LEARN by Alice\n LEARN by Tom");

            //possibly executable LEARN by Alice
            //LEARN by Tom
            query = new ExecutableQuery(program,
                logicClausesFactory.CreateEmptyLogicClause(), false);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "possibly executable LEARN by Alice\n LEARN by Tom");
        }

    }
}
