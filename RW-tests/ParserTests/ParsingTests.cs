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
        #region Clauses

        [TestMethod]
        public void FluentsParsingCOA()
        {
            string fluentName = "fluent";
            int fluentId = 2;
            string text = fluentName;
            Fluents = new Dictionary<string, int>();
            Fluents.Add(fluentName, fluentId);
            LogicClause lc = ParseText(text);
            Assert.IsInstanceOfType(lc, typeof(ConjunctionOfAlternatives));
            ConjunctionOfAlternatives coa = (ConjunctionOfAlternatives) lc;
            Assert.AreEqual(1, coa.Alternatives.Count);

            lc = ParseText("!"+text);
            Assert.IsInstanceOfType(lc, typeof(ConjunctionOfAlternatives));
            coa = (ConjunctionOfAlternatives)lc;
            Assert.AreEqual(1, coa.Alternatives.Count);
        }

        [TestMethod]
        public void OnlyAndsCOA()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c" };
            List<int> fluentIds = new List<int>() { 1, 2, 3 };
            string text = "!a & !b & c";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            LogicClause lc = ParseText(text);
            Assert.IsInstanceOfType(lc, typeof(ConjunctionOfAlternatives));
            ConjunctionOfAlternatives coa = (ConjunctionOfAlternatives)lc;
            Assert.AreEqual(fluentNames.Count, coa.Alternatives.Count);
        }

        [TestMethod]
        public void CNFWithoutBracketsCAO()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c" };
            List<int> fluentIds = new List<int>() { 1, 2, 3 };
            string text = "a | b | !c & a & c | !b & a";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            LogicClause lc = ParseText(text);
            Assert.IsInstanceOfType(lc, typeof(ConjunctionOfAlternatives));
            ConjunctionOfAlternatives coa = (ConjunctionOfAlternatives)lc;
            Assert.AreEqual(4, coa.Alternatives.Count);
            Alternative alt = coa.Alternatives[0];
            Assert.AreEqual(0x8, alt.NegatedFluents);
            Assert.AreEqual(0x6, alt.PositiveFluents);

        }

        [TestMethod]
        public void CNFWithBracketsCAO()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c" };
            List<int> fluentIds = new List<int>() { 1, 2, 3 };
            string text = "(a | b | !c) & a & (c | !b) & (a)";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            LogicClause lc = ParseText(text);
            Assert.IsInstanceOfType(lc, typeof(ConjunctionOfAlternatives));
            ConjunctionOfAlternatives coa = (ConjunctionOfAlternatives)lc;
            Assert.AreEqual(4, coa.Alternatives.Count);
            Alternative alt = coa.Alternatives[0];
            Assert.AreEqual(0x8, alt.NegatedFluents);
            Assert.AreEqual(0x6, alt.PositiveFluents);

        }

        [TestMethod]
        public void DNFWithBracketsAOC()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c" };
            List<int> fluentIds = new List<int>() { 1, 2, 3 };
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            string text = "(a & b & !c) | a | (c & !b)";
            LogicClause lc = ParseText(text);
            Assert.IsInstanceOfType(lc, typeof(AlternativeOfConjunctions));
            AlternativeOfConjunctions aoc = (AlternativeOfConjunctions)lc;
            Assert.AreEqual(3, aoc.Conjunctions.Count);
            Conjunction conj = aoc.Conjunctions[0];
            Assert.AreEqual(0x8, conj.NegatedFluents);
            Assert.AreEqual(0x6, conj.PositiveFluents);
        }
        #endregion //Clauses


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
            Assert.IsTrue(CheckIfCNF("!" + text));
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
