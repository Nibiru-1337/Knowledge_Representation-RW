using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend.Models.Parser
{
    public enum ClauseType
    {
        Conjuction, Alternative, Negation
    }

    public abstract class ParserClause
    {
        public ParserClause()
        {
        }
    
    }

    public abstract class ParserLogicClause : ParserClause
    {
        public List<ParserClause> Clauses { get; protected set; }
        public ParserLogicClause()
        {
            Clauses = new List<ParserClause>();
        }

        public void Add(ParserClause pc)
        {
            Clauses.Add(pc);
        }
    }
}
