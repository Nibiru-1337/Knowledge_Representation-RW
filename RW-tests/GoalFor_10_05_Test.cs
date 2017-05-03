using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend;
using RW_backend.Logic;
using RW_backend.Models;
using RW_Frontend;

namespace RW_tests
{
    [TestClass]
    public class GoalFor1005Test
    {
        [TestMethod]
        public void YaleScenarioTest()
        {
            var model = PrepareModel();
            var logic = new BackendLogic();
            ////dalej jak już będziemy mieć kwerendy, to coś w rodzaju
            var world = logic.CalculateWorld(model);
            var query = Query.Create("!alive after SHOOT by {Bob}");
            var queryResult = query.Evaluate(world);
            Assert.IsTrue(queryResult.IsTrue);
            Assert.IsNotNull(queryResult.Function);
            Assert.AreEqual(new State(0x3), queryResult.Function[0]);//alive,loaded
            Assert.AreEqual(new State(0), queryResult.Function[1]);//!alive,!loaded
        }

        private static Model PrepareModel()
        {
            var vm = VM.Create(noninertial: new string[0],
                fluents: new[] { "loaded", "alive" },
                actions: new[] { "LOAD", "SHOOT" },
                agents: new[] { "Bob" },
                always: new string[0],
                initially: new[] { "loaded", "alive" },
                after: new string[0],
                causes: new[] { "SHOOT by {Bob} causes !alive if loaded" },
                releases: new string[0]
            );

            var converter = new ModelConverter();
            var model = converter.ConvertToModel(vm);
            return model;
        }
    }
}
