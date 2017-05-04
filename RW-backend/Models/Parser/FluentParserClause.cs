using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Parser
{
    public class FluentParserClause : ParserClause
    {
        public bool IsNegation;
        public int Index;

        public FluentParserClause(int index, bool isNegation=false)
        {
            Index = index;
            IsNegation = isNegation;
        }
    }
}
