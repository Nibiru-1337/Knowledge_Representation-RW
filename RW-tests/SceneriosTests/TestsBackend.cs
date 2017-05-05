using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_tests.SceneriosTests
{
    [TestClass]
    public class TestsBackend
    {
        [TestMethod]
        public void ExampleWorldTest()
        {
            //world with 2 fluents ex. alive=0, loaded=1
            World world = new World(2);
            
            //loaded is set after LOAD
            Alternative effect = new Alternative();
            effect.AddFluent(1, false);
            Causes LOAD = new Causes(null, effect, 0x1, 0x0);
            world.AddCauses(LOAD);

            //should have an edge from state alive, -loaded -> alive, loaded
            List<State> afterLOAD = world.Connections[new Tuple<Causes, State>(LOAD, new State(0x1))];
            Assert.AreEqual(1, afterLOAD.Count);
            Assert.AreEqual(new State(0x3), afterLOAD[0]);
            //should have an edge from state alive, loaded -> alive, loaded
            afterLOAD = world.Connections[new Tuple<Causes, State>(LOAD, new State(0x3))];
            Assert.AreEqual(1, afterLOAD.Count);
            Assert.AreEqual(new State(0x3), afterLOAD[0]);
            //should have an edge from state -alive, loaded -> -alive, loaded
            afterLOAD = world.Connections[new Tuple<Causes, State>(LOAD, new State(0x2))];
            Assert.AreEqual(1, afterLOAD.Count);
            Assert.AreEqual(new State(0x2), afterLOAD[0]);
            //should have an edge from state -alive, -loaded -> -alive, loaded
            afterLOAD = world.Connections[new Tuple<Causes, State>(LOAD, new State(0x0))];
            Assert.AreEqual(1, afterLOAD.Count);
            Assert.AreEqual(new State(0x2), afterLOAD[0]);

            //-alive after SHOOT with loaded condition by agent with index = 0 (bob or whatever)
            Alternative conditions = new Alternative();
            conditions.AddFluent(1, false);
            Conjunction effect2 = new Conjunction();
            effect2.AddFluent(0, true);
            effect2.AddFluent(1, true);
            Causes SHOOT = new Causes(conditions, effect2, 0x2, 0x1);
            world.AddCauses(SHOOT);

            //should have an edge from state alive, loaded -> -alive, -loaded
            List<State> afterSHOOT = world.Connections[new Tuple<Causes, State>(SHOOT, new State(0x3))];
            Assert.AreEqual(1, afterLOAD.Count);
            Assert.AreEqual(new State(0x0), afterSHOOT[0]);
            //should have an edge from state -alive, loaded -> -alive, -loaded
            afterSHOOT = world.Connections[new Tuple<Causes, State>(SHOOT, new State(0x2))];
            Assert.AreEqual(1, afterLOAD.Count);
            Assert.AreEqual(new State(0x0), afterSHOOT[0]);
            //should not have edges from state alive, -loaded
            try
            {
                afterSHOOT = world.Connections[new Tuple<Causes, State>(SHOOT, new State(0x1))];
                Assert.Fail();
            } catch (KeyNotFoundException) { }

            //should not have edges from alive, -loaded
            try
            {
                afterSHOOT = world.Connections[new Tuple<Causes, State>(SHOOT, new State(0x0))];
                Assert.Fail();
            } catch(KeyNotFoundException) { }
            
        }
    }
}
