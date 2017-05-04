using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Parser;

namespace RW_tests.ParserTests
{
    [TestClass]
    public class ParsingTests : Parser
    {
        public ParsingTests() : base(new Dictionary<string, int>())
        {

        }
        #region CNF checking

        [TestMethod]
        public void FluentsParsingCheck()
        {
            string fluentName = "fluent";
            int fluentId = 2;
            string text = fluentName;
            Fluents = new Dictionary<string, int>();
            Fluents.Add(fluentName, fluentId);
            Assert.IsTrue(CheckIfCNF(text));
            Assert.IsTrue(CheckIfCNF("!"+text));
        }

        [TestMethod]
        public void OneOperatorsType()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c" };
            List<int> fluentIds = new List<int>() { 1, 2, 3 };
            string text = "a | b | !c";
            string text2 = "!a & !b & c";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            Assert.IsFalse(CheckIfCNF(text));
            Assert.IsTrue(CheckIfCNF(text2));
        }

        [TestMethod]
        public void DifferentOperatorsType()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c" };
            List<int> fluentIds = new List<int>() { 1, 2, 3 };
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            string text = "a | b | !c & a & c | !b";
            Assert.IsTrue(CheckIfCNF(text));
        }

        [TestMethod]
        public void CNFWithBrackets()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c" };
            List<int> fluentIds = new List<int>() { 1, 2, 3 };
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            string text = "(a | b | !c) & a & (c | !b)";
            string text2 = "a & (c | !b)";
            Assert.IsTrue(CheckIfCNF(text));
            Assert.IsTrue(CheckIfCNF(text2));
        }

        [TestMethod]
        public void DNFWithBrackets()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c" };
            List<int> fluentIds = new List<int>() { 1, 2, 3 };
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            string text = "(a & b & !c) | a | (c & !b)";
            string text2 = "a | (c & !b)";
            Assert.IsFalse(CheckIfCNF(text));
            Assert.IsFalse(CheckIfCNF(text2));
        }

        [TestMethod]
        public void SimpleNotDefinedFluentParsing()
        {
            string fluentName = "fluent";
            AssertExeption(fluentName);
        }
        #endregion //CNF checking

        protected void AssertExeption(string text)
        {
            try
            {
                LogicClause pc = ParseText(text);
                Assert.Fail();
            }
            catch (ArgumentException)
            {

            }
        }
    }
}
