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
            AlternativeOfConjunctions effect = new AlternativeOfConjunctions();
            Conjunction c = new Conjunction();
            //loaded is set after action
            c.AddFluent(1,false);
            effect.AddConjunction(c);
            Causes a = new Causes(null,effect,1,0);
            world.AddCauses(a);
        }
    }
}
