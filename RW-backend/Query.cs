using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_backend
{
    /// <summary>
    /// Reprezentacja kwerendy
    /// </summary>
    abstract class Query
    {
        public enum QueryType
        {
            Executable,
            After,
            Engaged
        }
        public abstract QueryType Type { get; }
        
        public abstract QueryResult Evaluate(World world);

        //opcjonalnie tworzenie kwerend na podstawie zdania
        public static Query Create(string queryString)
        {
            throw new NotImplementedException();
        }
    }

    class QueryResult
    {
        public bool IsTrue;
        public List<State> Function;
    }

    //TODO zaimplementować typy odpowiadające kwerendom executable, after i engaged
    class ExectutableQuery : Query
    {
        public override QueryType Type { get { return QueryType.Executable; } }
        public override QueryResult Evaluate(World world)
        {
            throw new NotImplementedException();
        }
    }

    class AfterQuery : Query
    {
        public override QueryType Type { get { return QueryType.After; } }
        public override QueryResult Evaluate(World world)
        {
            throw new NotImplementedException();
        }
    }

    class EngagedQuery : Query
    {
        public override QueryType Type { get { return QueryType.Engaged; } }
        public override QueryResult Evaluate(World world)
        {
            throw new NotImplementedException();
        }
    }
}
