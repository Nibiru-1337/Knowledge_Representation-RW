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
