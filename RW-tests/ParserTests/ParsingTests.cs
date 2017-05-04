using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RW_backend.Models.Parser;

namespace RW_tests.ParserTests
{
    [TestClass]
    public class ParsingTests : Parser
    {
        public ParsingTests() : base(new Dictionary<string, int>())
        {

        }
        #region Fluents
        [TestMethod]
        public void SimpleFluentParsing()
        {
            string fluentName = "fluent";
            int fluentId = 2;
            Fluents = new Dictionary<string, int>();
            Fluents.Add(fluentName, fluentId);
            ParserClause pc = ParseText(fluentName);

            Assert.IsInstanceOfType(pc, typeof(FluentParserClause));
            FluentParserClause fpc = (FluentParserClause)pc;
            Assert.AreEqual(fluentId, fpc.Index);
            Assert.IsFalse(fpc.IsNegation);
        }

        [TestMethod]
        public void SimpleNoFluentParsing()
        {
            string fluentName = "fluent";
            Fluents = new Dictionary<string, int>();
            AssertExeption(fluentName);
        }


        [TestMethod]
        public void SimpleNotFluentParsing()
        {
            string fluentName = "fluent";
            int fluentId = 2;
            Fluents = new Dictionary<string, int>();
            Fluents.Add(fluentName, fluentId);
            ParserClause pc = ParseText("!" + fluentName);

            Assert.IsInstanceOfType(pc, typeof(FluentParserClause));
            FluentParserClause fpc = (FluentParserClause)pc;
            Assert.AreEqual(fluentId, fpc.Index);
            Assert.IsTrue(fpc.IsNegation);
        }

        [TestMethod]
        public void SimpleNoNotFluentParsing()
        {
            string fluentName = "fluent";
            Fluents = new Dictionary<string, int>();
            AssertExeption("!" + fluentName);
        }
        #endregion //Fluents
        #region Ory
        [TestMethod]
        public void SimpleOrParsing()
        {
            List<string> fluentNames = new List<string>() { "a", "b" };
            List<int> fluentIds = new List<int>() { 1, 2 };
            string text = "a | b";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(OrParserClause));
            OrParserClause opc = (OrParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(0, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void SimpleOrParsingWithNegationAtTheBeginning()
        {
            List<string> fluentNames = new List<string>() { "a", "b" };
            List<int> fluentIds = new List<int>() { 1, 2 };
            string text = "!a | b";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(OrParserClause));
            OrParserClause opc = (OrParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(1, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void SimpleManyFluentsOrParsing()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c", "d", "e", "f", "g" };
            List<int> fluentIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            string text = " a | b |c|d|e  |f|g  ";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(OrParserClause));
            OrParserClause opc = (OrParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(0, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void SimpleManyNegatedFluentsOrParsing()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c", "d", "e", "f", "g" };
            List<int> fluentIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            string text = " !a |! b |!c|!d|!e  | !f|!g  ";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(OrParserClause));
            OrParserClause opc = (OrParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void OrParsing()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c", "d", "e", "f", "g" };
            List<int> fluentIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            string text = " !a | b |!c|d|!e  | !f|g  ";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(OrParserClause));
            OrParserClause opc = (OrParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(4, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void SimpleOrParsingWithNegationAtTheEnd()
        {
            List<string> fluentNames = new List<string>() { "a", "b" };
            List<int> fluentIds = new List<int>() { 1, 2 };
            string text = "a | !b";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(OrParserClause));
            OrParserClause opc = (OrParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(1, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }
        #endregion //ory

        #region Andy
        [TestMethod]
        public void SimpleArdParsing()
        {
            List<string> fluentNames = new List<string>() { "a", "b" };
            List<int> fluentIds = new List<int>() { 1, 2 };
            string text = "a & b";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(AndParserClause));
            AndParserClause opc = (AndParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(0, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void SimpleAndParsingWithNegationAtTheBeginning()
        {
            List<string> fluentNames = new List<string>() { "a", "b" };
            List<int> fluentIds = new List<int>() { 1, 2 };
            string text = "!a & b";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(AndParserClause));
            AndParserClause opc = (AndParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(1, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void SimpleManyFluentsAndParsing()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c", "d", "e", "f", "g" };
            List<int> fluentIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            string text = " a & b &c&d&e  &f&g  ";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(AndParserClause));
            AndParserClause opc = (AndParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(0, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void SimpleManyNegatedFluentsAndParsing()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c", "d", "e", "f", "g" };
            List<int> fluentIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            string text = " !a &! b &!c&!d&!e  & !f&!g  ";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(AndParserClause));
            AndParserClause opc = (AndParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void AndParsing()
        {
            List<string> fluentNames = new List<string>() { "a", "b", "c", "d", "e", "f", "g" };
            List<int> fluentIds = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            string text = " !a & b &!c&d&!e  & !f&g  ";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(AndParserClause));
            AndParserClause opc = (AndParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(4, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }

        [TestMethod]
        public void SimpleAndParsingWithNegationAtTheEnd()
        {
            List<string> fluentNames = new List<string>() { "a", "b" };
            List<int> fluentIds = new List<int>() { 1, 2 };
            string text = "a & !b";
            Fluents = new Dictionary<string, int>();
            for (int i = 0; i < fluentNames.Count; i++)
            {
                Fluents.Add(fluentNames[i], fluentIds[i]);
            }
            ParserClause pc = ParseText(text);

            Assert.IsInstanceOfType(pc, typeof(AndParserClause));
            AndParserClause opc = (AndParserClause)pc;
            Assert.AreEqual(fluentNames.Count, opc.Clauses.Count);
            Assert.AreEqual(1, opc.Clauses.Count(
                x =>
                x.GetType() == typeof(FluentParserClause) && ((FluentParserClause)x).IsNegation
                ));
        }
        #endregion //Andy
        protected void AssertExeption(string text)
        {
            try
            {
                ParserClause pc = ParseText(text);
                Assert.Fail();
            }
            catch (ErrorException)
            {

            }
        }
    }
}
