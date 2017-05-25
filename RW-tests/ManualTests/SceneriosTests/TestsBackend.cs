using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.World;

namespace RW_tests.SceneriosTests
{
    [TestClass]
    public class TestsBackend
    {

        private World _SetUpSimpleWorld()
        {
            

            //add 2 actions, LOAD and SHOOT executed by Bob
            //LOADID=0, SHOOTID=1

            IList<Causes> causesList = new List<Causes>();
            //loaded is set after LOAD by agent with index = 0 (bob or whatever)
            UniformAlternative effect = new UniformAlternative();
            effect.AddFluent(1, FluentSign.Positive);
            Causes LOAD = new Causes(null, effect, 0x0, new AgentsSet(0x1));
            causesList.Add(LOAD);

            //-alive after SHOOT with loaded condition by agent with index = 0 (bob or whatever)
            UniformAlternative conditions = new UniformAlternative();
            conditions.AddFluent(1, FluentSign.Positive);
            UniformConjunction effect2 = new UniformConjunction();
            effect2.AddFluent(0, FluentSign.Negated);
            effect2.AddFluent(1, FluentSign.Negated);
            Causes SHOOT = new Causes(conditions, effect2, 0x1, new AgentsSet(0x1));
            causesList.Add(SHOOT);

			//world with 2 fluents ex. alive=0, loaded=1 sand 1 agent Bob=0
			World world = new World(2, new List<LogicClause>(), new List<LogicClause>(), null,causesList, null, null, 2);

            return world;
        }

        [TestMethod]
        public void AlwaysClausesSatisfied()
        {
            IList<LogicClause> alwaysList = new List<LogicClause>();
            UniformAlternative x = new UniformAlternative();
            x.AddFluent(3, FluentSign.Positive);
            alwaysList.Add(x);
            // always idx=3
            World world = new World(4, alwaysList, new List<LogicClause>(), null, null, null, null, 0);

            Assert.AreEqual(8, world.States.Count());

            alwaysList.Clear();
            UniformConjunction y = new UniformConjunction();
            y.AddFluent(0, FluentSign.Negated);
            y.AddFluent(1, FluentSign.Negated);
            alwaysList.Add(y);
            // always -idx=0 ^ -idx=1
            world = new World(4, alwaysList, new List<LogicClause>(), null, null, null, null, 0);

            Assert.AreEqual(4, world.States.Count());

            alwaysList.Add(x);
            world = new World(4, alwaysList, new List<LogicClause>(), null, null, null, null, 0);

            Assert.AreEqual(2, world.States.Count());
        }
    }
}
