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
using RW_tests.UltimateSystemTests;

namespace RW_tests.UltimateSystemTests.EngagedTests
{
    [TestClass]
    public class EngagedTests
    {
        [TestMethod]
        public void EngagedBobTest()
        {
            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            World world = new BackendLogic().CalculateWorld(model);
            BitSetFactory bitSetFactory = new BitSetFactory();
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Tom, ScenarioConsts.Alice }));
            program.Add(aap);

            EngagedQuery query;
            AgentsSet whoisEngaged = new AgentsSet(bitSetFactory.CreateFromOneElement(ScenarioConsts.Bob));
            //Bob always engaged in LEARN by Alice, Tom, Bob
            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob always engaged in LEARN by Alice, Tom, Bob");
            //Bob possibly engaged in LEARN by Alice, Tom, Bob
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob possibly engaged in LEARN by Alice, Tom, Bob");

            //Bob always engaged in LEARN by Alice, Bob
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Alice }));
            program.Clear();
            program.Add(aap);
            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob always engaged in LEARN by Alice, Bob");
            //Bob possibly engaged in LEARN by Alice, Bob
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob possibly engaged in LEARN by Alice, Bob");

            //Bob always engaged in LEARN by Alice, Bob from ~Math
            query = new EngagedQuery(program, 
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Negated), true, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob always engaged in LEARN by Alice, Bob from ~Math");
            //Bob possibly engaged in LEARN by Alice, Bob from ~Math
            query = new EngagedQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Negated), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob possibly engaged in LEARN by Alice, Bob from ~Math");
        }

        [TestMethod]
        public void EngagedBobTomTest()
        {
            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            World world = new BackendLogic().CalculateWorld(model);
            BitSetFactory bitSetFactory = new BitSetFactory();
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Tom, ScenarioConsts.Alice }));
            program.Add(aap);

            EngagedQuery query;
            AgentsSet whoisEngaged = new AgentsSet(bitSetFactory.CreateBitSetValueFrom(new List<int> {ScenarioConsts.Bob, ScenarioConsts.Tom}));
            
            //Bob, Tom always engaged in LEARN by Alice, Tom, Bob
            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob, Tom always engaged in LEARN by Alice, Tom, Bob");
            //Bob, Tom possibly engaged in LEARN by Alice, Tom, Bob
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob, Tom possibly engaged in LEARN by Alice, Tom, Bob");

            //Bob, Tom always engaged in LEARN by Alice, Tom, Bob from ~Math
            query = new EngagedQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Negated), true, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob, Tom always engaged in LEARN by Alice, Tom, Bob from ~Math");
            //Bob, Tom possibly engaged in LEARN by Alice, Tom, Bob from ~Math
            query = new EngagedQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Negated), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob, Tom possibly engaged in LEARN by Alice, Tom, Bob from ~Math");

            //Bob, Tom always engaged in LEARN by Bob, Tom
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Tom }));
            program.Clear();
            program.Add(aap);
            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob, Tom always engaged in LEARN by Bob, Tom");
            //Bob, Tom possibly engaged in LEARN by Bob, Tom
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob, Tom possibly engaged in LEARN by Bob, Tom");

            //Bob, Tom always engaged in LEARN by Bob
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob }));
            program.Clear();
            program.Add(aap);
            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob, Tom always engaged in LEARN by Bob");
            //Bob, Tom possibly engaged in LEARN by Bob
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Bob, Tom possibly engaged in LEARN by Bob");

            //Bob, Tom always engaged in LEARN by Alice
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Alice }));
            program.Clear();
            program.Add(aap);
            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob, Tom always engaged in LEARN by Alice");
            //Bob, Tom possibly engaged in LEARN by Alice
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Bob, Tom possibly engaged in LEARN by Alice");
        }

        [TestMethod]
        public void EngagedAlice()
        {
            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            World world = new BackendLogic().CalculateWorld(model);
            BitSetFactory bitSetFactory = new BitSetFactory();
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Alice }));
            program.Add(aap);
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Jack, ScenarioConsts.Bob }));
            program.Add(aap);

            EngagedQuery query;
            AgentsSet whoisEngaged = new AgentsSet(bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Alice }));

            //Alice always engaged in (LEARN by Alice, LEARN by Jack, Bob)
            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Alice always engaged in (LEARN by Alice, LEARN by Jack, Bob)");
            //Alice possibly engaged in (LEARN by Alice, LEARN by Jack, Bob)
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Alice possibly engaged in (LEARN by Alice, LEARN by Jack, Bob)");

            //Alice always engaged in (LEARN by Alice , LEARN by Jack, Bob) from ~HasToy^~Math
            query = new EngagedQuery(program, 
                UniformConjunction.CreateFrom(new List<int>(), new List<int> {ScenarioConsts.HasToy, ScenarioConsts.Math}),
                true, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Alice always engaged in (LEARN by Alice , LEARN by Jack, Bob) from ~HasToy^~Math");

            //Alice possibly engaged in (LEARN by Alice , LEARN by Jack, Bob) from ~HasToy^~Math
            query = new EngagedQuery(program,
                UniformConjunction.CreateFrom(new List<int>(), new List<int> { ScenarioConsts.HasToy, ScenarioConsts.Math }),
                false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Alice possibly engaged in (LEARN by Alice , LEARN by Jack, Bob) from ~HasToy^~Math");

        }

        [TestMethod]
        public void JackEngaged()
        {
            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            World world = new BackendLogic().CalculateWorld(model);
            BitSetFactory bitSetFactory = new BitSetFactory();
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Jack, ScenarioConsts.Bob }));
            program.Add(aap);

            EngagedQuery query;
            AgentsSet whoisEngaged = new AgentsSet(bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Jack }));

            //Jack always engaged in LEARN by Jack, Bob
            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Jack always engaged in LEARN by Jack, Bob");
            //Jack possibly engaged in LEARN by Jack, Bob
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Jack possibly engaged in LEARN by Jack, Bob");

            //Jack always engaged in LEARN by Jack, Bob from HasToy
            query = new EngagedQuery(program, 
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Positive), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Jack always engaged in LEARN by Jack, Bob from HasToy");
            //Jack possibly engaged in LEARN by Jack, Bob from HasToy
            query = new EngagedQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Positive), false, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Jack always engaged in LEARN by Jack, Bob from HasToy");

            //Jack always engaged in LEARN by Jack, Bob from ~HasToy
            query = new EngagedQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Negated), true, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Jack always engaged in LEARN by Jack, Bob from ~HasToy");
            //Jack possibly engaged in LEARN by Jack, Bob from ~HasToy
            query = new EngagedQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Negated), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Jack possibly engaged in LEARN by Jack, Bob from ~HasToy");

            //Jack engaged in LEARN by Jack, Bob from ~HasToy ^ ~Math
            query = new EngagedQuery(program,
                UniformConjunction.CreateFrom(new List<int>(),new List<int> {ScenarioConsts.HasToy, ScenarioConsts.Math} ),
                true, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Jack always engaged in LEARN by Jack, Bob from ~HasToy ^ ~Math");
            //Jack possibly in LEARN by Jack, Bob from ~HasToy ^ ~Math
            query = new EngagedQuery(program,
                UniformConjunction.CreateFrom(new List<int>(), new List<int> { ScenarioConsts.HasToy, ScenarioConsts.Math }),
                false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Jack possibly engaged in LEARN by Jack, Bob from ~HasToy ^ ~Math");

        }

        [TestMethod]
        public void TomEngaged()
        {
            Model model = PatriciaExamSessionScenratioGenerator.GenerateModel();
            World world = new BackendLogic().CalculateWorld(model);
            BitSetFactory bitSetFactory = new BitSetFactory();
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            List<ActionAgentsPair> program = new List<ActionAgentsPair>();
            ActionAgentsPair aap;
            aap = new ActionAgentsPair(ScenarioConsts.Drink,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Tom, ScenarioConsts.Bob, ScenarioConsts.Alice }));
            program.Add(aap);

            EngagedQuery query;
            AgentsSet whoisEngaged = new AgentsSet(bitSetFactory.CreateBitSetValueFrom(new List<int> { ScenarioConsts.Tom }));

            //Tom always engaged in DRINK by Alice, Tom, Bob
            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Tom always engaged in DRINK by Alice, Tom, Bob");
            //Tom possibly engaged in DRINK by Alice, Tom, Bob
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Tom possibly engaged in DRINK by Alice, Tom, Bob");

            //Tom always engaged in (LEARN by Alice, Bob,  DRINK by Tom)
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Bob, ScenarioConsts.Alice }));
            program.Clear();
            program.Add(aap);
            aap = new ActionAgentsPair(ScenarioConsts.Drink,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Tom }));
            program.Add(aap);

            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Tom always engaged in (LEARN by Alice, Bob,  DRINK by Tom)");
            //Tom possibly engaged in (LEARN by Alice, Bob,  DRINK by Tom)
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Tom possibly engaged in (LEARN by Alice, Bob,  DRINK by Tom)");

            //Tom always engaged in (DRINK by Tom, LEARN by Alice)
            aap = new ActionAgentsPair(ScenarioConsts.Drink,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Tom }));
            program.Clear();
            program.Add(aap);
            aap = new ActionAgentsPair(ScenarioConsts.Learn,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Alice }));
            program.Add(aap);

            query = new EngagedQuery(program, new UniformAlternative(), true, whoisEngaged);
            Assert.AreEqual(false, query.Evaluate(world).IsTrue, "Tom always engaged in (DRINK by Tom, LEARN by Alice)");
            //Tom possibly engaged in (DRINK by Tom, LEARN by Alice)
            query = new EngagedQuery(program, new UniformAlternative(), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Tom always possibly in (DRINK by Tom, LEARN by Alice)");

            //Tom always engaged in DRINK by Tom from Math
            aap = new ActionAgentsPair(ScenarioConsts.Drink,
                bitSetFactory.CreateBitSetValueFrom(new List<int>() { ScenarioConsts.Tom }));
            program.Clear();
            program.Add(aap);

            query = new EngagedQuery(program, 
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive), true, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Tom always engaged in DRINK by Tom from Math");
            //Tom possibly engaged in DRINK by Tom from Math
            query = new EngagedQuery(program,
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive), false, whoisEngaged);
            Assert.AreEqual(true, query.Evaluate(world).IsTrue, "Tom possibly engaged in DRINK by Tom from Math");
        }
    }
}
