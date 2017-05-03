using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend;

namespace RW_tests
{
    [TestClass]
    public class TestsBackend
    {
        [TestMethod]
        public void Simple_Yale_Shooting_Problem_Test()
        {
			//TODO: adjust
            OrderedDictionary fluents = new OrderedDictionary();
            //initial fluents
            fluents.Add("loaded", false);
            fluents.Add("alive", true);

            //create LOAD action
            //LOAD has no conditions for execution
            Dictionary<string, bool> conditions = new Dictionary<string, bool>();
            //LOAD execution causes loaded
            Dictionary<string, bool> results = new Dictionary<string, bool>();
            results.Add("loaded", true);
            //RwAction LOAD = new RwAction(conditions, results);

            //create SHOOT action
            //SHOOT has conditions for execution
            conditions = new Dictionary<string, bool>();
            conditions.Add("loaded", true);
            //SHOOT execution causes loaded
            results = new Dictionary< string, bool> ();
            results.Add("loaded", false);
            results.Add("alive", false);
           // RwAction SHOOT = new RwAction(conditions, results);

            //List<RwAction> Actions = new List<RwAction>(2);
           // Actions.Add(SHOOT);
            //Actions.Add(LOAD);

            List<OrderedDictionary> nodes = WorldOperations.GenerateWorldNodes(fluents);
            Assert.AreEqual(nodes.Count, 4);
            //List<OrderedDictionary> ConnectedStates = WorldOperations.Resolution(LOAD, fluents, nodes);
            //Assert.AreEqual(ConnectedStates[0]["alive"], true);
            //Assert.AreEqual(ConnectedStates[0]["loaded"], true);
            //ConnectedStates = WorldOperations.Resolution(SHOOT, ConnectedStates[0], nodes);
            //Assert.AreEqual(ConnectedStates[0]["alive"], false);
            //Assert.AreEqual(ConnectedStates[0]["loaded"], false);
        }
    }
}
