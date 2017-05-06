using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.GraphModels;

namespace RW_tests.SceneriosTests
{
    [TestClass]
    public class TestsBackend
    {
        private World _SetUpWorld()
        {
            //world with 2 fluents ex. alive=0, loaded=1 sand 1 agent Bob=0
            World world = new World(2);

            //add 2 actions, LOAD and SHOOT executed by Bob
            //LOADID=0, SHOOTID=1

            //loaded is set after LOAD by agent with index = 0 (bob or whatever)
            UniformAlternative effect = new UniformAlternative();
            effect.AddFluent(1, false);
            Causes LOAD = new Causes(null, effect, 0x0, new AgentsSet(0x1));
            world.AddCauses(LOAD);

            //-alive after SHOOT with loaded condition by agent with index = 0 (bob or whatever)
            UniformAlternative conditions = new UniformAlternative();
            conditions.AddFluent(1, false);
            UniformConjunction effect2 = new UniformConjunction();
            effect2.AddFluent(0, true);
            effect2.AddFluent(1, true);
            Causes SHOOT = new Causes(conditions, effect2, 0x1, new AgentsSet(0x1));
            world.AddCauses(SHOOT);

            return world;
        }

        [TestMethod]
        public void WorldWithLoadedAliveAndBob()
        {
            //LOAD is actionId = 0 , SHOOT is actionId = 1
            World world = _SetUpWorld();

            //(LOAD) should have an edge from state alive, -loaded -> alive, loaded
            AgentSetChecker asc = world.Connections[new Tuple<int, State>(world.ActionIds[0], new State(0x1))];
            if (asc.Check(0x0)) Assert.Fail(); //empty agent set should not execute LOAD
            if (asc.Check(0x1))
            {
                List<State> afterLOAD = asc.edges;
                Assert.AreEqual(1, afterLOAD.Count);
                Assert.AreEqual(new State(0x3), afterLOAD[0]);
            }
            //(LOAD) should have an edge from state alive, loaded -> alive, loaded
            asc = world.Connections[new Tuple<int, State>(world.ActionIds[0], new State(0x3))];
            if (asc.Check(0x2)) Assert.Fail(); //Bob is not present, not executable
            if (asc.Check(0x1))
            {
                List<State> afterLOAD = asc.edges;
                Assert.AreEqual(1, afterLOAD.Count);
                Assert.AreEqual(new State(0x3), afterLOAD[0]);
            }
            //(LOAD) should have an edge from state -alive, loaded -> -alive, loaded
            asc = world.Connections[new Tuple<int, State>(world.ActionIds[0], new State(0x2))];
            if (asc.Check(0x3)) //Bob is present, executable
            {
                List<State> afterLOAD = asc.edges;
                Assert.AreEqual(1, afterLOAD.Count);
                Assert.AreEqual(new State(0x2), afterLOAD[0]);
            }
            else
            {
                Assert.Fail();
            }
            //(LOAD) should have an edge from state -alive, -loaded -> -alive, loaded
            asc = world.Connections[new Tuple<int, State>(world.ActionIds[0], new State(0x0))];
            if (asc.Check(0x1)) {
                List<State> afterLOAD = asc.edges;
                Assert.AreEqual(1, afterLOAD.Count);
                Assert.AreEqual(new State(0x2), afterLOAD[0]);
            }
            //(SHOOT) should have an edge from state alive, loaded -> -alive, -loaded
            asc = world.Connections[new Tuple<int, State>(world.ActionIds[1], new State(0x3))];
            if (asc.Check(0x1))
            {
                List<State> afterSHOOT = asc.edges;
                Assert.AreEqual(1, afterSHOOT.Count);
                Assert.AreEqual(new State(0x0), afterSHOOT[0]);
            }
            //(SHOOT) should have an edge from state -alive, loaded -> -alive, -loaded
            asc = world.Connections[new Tuple<int, State>(world.ActionIds[1], new State(0x2))];
            if (asc.Check(0x1))
            {
                List<State> afterSHOOT = asc.edges;
                Assert.AreEqual(1, afterSHOOT.Count);
                Assert.AreEqual(new State(0x0), afterSHOOT[0]);
            }
            //(SHOOT) should not have edges from state alive, -loaded
            try
            {
                asc = world.Connections[new Tuple<int, State>(world.ActionIds[1], new State(0x1))];
                Assert.Fail();
            } catch (KeyNotFoundException) { }

            //(SHOOT) should not have edges from alive, -loaded
            try
            {
                asc = world.Connections[new Tuple<int, State>(world.ActionIds[1], new State(0x0))];
                Assert.Fail();
            } catch(KeyNotFoundException) { }
            
        }
    }
}
