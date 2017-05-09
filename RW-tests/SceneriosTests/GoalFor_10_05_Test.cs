using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Logic;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.World;
using RW_Frontend;
using RW_Frontend.InputsViewModels;

namespace RW_tests.SceneriosTests
{
    [TestClass]
    public class GoalFor1005Test
    {
        private static World PrepareWorld(VM vm)
        {
            var model = PrepareModel(vm);
            var logic = new BackendLogic();
            var world = logic.CalculateWorld(model);
            return world;
        }

        /// <summary>
        /// Sprawdź wykonanie YSP ze stanu (alive,loaded) po akcji SHOOT do (!alive,!loaded)
        /// </summary>
        [TestMethod]
        public void YaleScenarioTest()
        {
            var vm = PrepareVM();
            var world = PrepareWorld(vm);
            var query = PrepareAfterQuery_Shoot(vm, false, "alive&loaded");
            var queryResult = query.Evaluate(world);
            Assert.IsTrue(queryResult.IsTrue, "query should be true");
            Assert.IsNotNull(queryResult.StatePath, "function should not be null");
            Assert.AreEqual(new State(0x3), queryResult.StatePath[0]);//alive,loaded
            Assert.AreEqual(new State(0), queryResult.StatePath[1]);//!alive,!loaded
        }

        /// <summary>
        /// Sprawdź wartość kwerendy possibly !alive after SHOOT by Bob from (alive,loaded)
        /// </summary>
        [TestMethod]
        public void YaleScenarioSuccesOnlyTest_ShootLoaded()
        {
            var vm = PrepareVM();
            var world = PrepareWorld(vm);
            var query = PrepareAfterQuery_Shoot(vm, false, "alive&loaded");
            var queryResult = query.Evaluate(world);
            Assert.IsTrue(queryResult.IsTrue, "query should be true - SHOOT with loaded");
        }

        /// <summary>
        /// Sprawdź wartość kwerendy possibly !alive after SHOOT by Bob from (alive,!loaded)
        /// </summary>
        [TestMethod]
        public void YaleScenarioSuccesOnlyTest_ShootNotLoaded()
        {
            var vm = PrepareVM();
            var world = PrepareWorld(vm);
            var query = PrepareAfterQuery_Shoot(vm, false, "alive&!loaded");
            var queryResult = query.Evaluate(world);
            Assert.IsFalse(queryResult.IsTrue, "query should not be true - SHOOT with !loaded");
        }


        /// <summary>
        /// Sprawdź wartość kwerend possibly/necessary !alive after SHOOT by Bob from (alive)
        /// </summary>
        [TestMethod]
        public void YaleScenarioSuccesOnlyTest_Shoot()
        {
            var vm = PrepareVM();
            var world = PrepareWorld(vm);
            var query = PrepareAfterQuery_Shoot(vm, false, "alive");
            var queryResult = query.Evaluate(world);
            Assert.IsTrue(queryResult.IsTrue, "possibly query should be true");
            query = PrepareAfterQuery_Shoot(vm, true, "");
            queryResult = query.Evaluate(world);
            Assert.IsFalse(queryResult.IsTrue, "necessary query should not be true");
        }

        private static AfterQuery PrepareAfterQuery_Shoot(VM vm, bool always, string initial)
        {
            var fluentsViewModels = vm.Fluents.Select(fluent => new FluentViewModel(fluent)).ToList();
            var actionsViewModels = vm.Actions.Select(action => new ActionViewModel(action)).ToList();
            var agentsViewModels = vm.Agents.Select(agent => new AgentViewModel(agent)).ToList();
            return new ModelConverter().ConvertAfterQuery(CreateAfterQueryVM_Shoot(always, initial), agentsViewModels, actionsViewModels, fluentsViewModels);
        }

        private static AfterQueryViewModel CreateAfterQueryVM_Shoot(bool always, string initial)
        {
            var after = new AfterQueryViewModel(always ? AfterQueryViewModel.AfterQueryNecOrPos.Necessary : AfterQueryViewModel.AfterQueryNecOrPos.Possibly,
                "!alive", new List<Tuple<string, List<string>>>() { new Tuple<string, List<string>>("SHOOT", new List<string>() { "Bob" }) }, initial);
            return after;
        }

        private static Model PrepareModel(VM vm)
        {
            var fluents = vm.Fluents.Select(fluent => new FluentViewModel(fluent)).ToList();
            var actions = vm.Actions.Select(action => new ActionViewModel(action)).ToList();
            var agents = vm.Agents.Select(agent => new AgentViewModel(agent)).ToList();

            var causes = new List<CausesClauseViewModel> { new CausesClauseViewModel("SHOOT", new List<string> { "Bob" }, "!alive", "loaded"), new CausesClauseViewModel("SHOOT", new List<string> { "Bob" }, "!loaded", "") };

            var converter = new ModelConverter();
            var model = converter.ConvertToModel(fluents, actions, agents, causes);
            return model;
        }

        private static VM PrepareVM()
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
            return vm;
        }
    }
}
