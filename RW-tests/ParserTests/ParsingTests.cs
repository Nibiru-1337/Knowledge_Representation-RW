using System;
using System.Collections.Generic;
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
            FluentParserClause fpc = (FluentParserClause) pc;
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
            ParserClause pc = ParseText("!"+fluentName);

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
            AssertExeption("!"+fluentName);
        }
        #endregion //Fluents

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
