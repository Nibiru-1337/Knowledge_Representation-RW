using System;
using System.Collections.Generic;

namespace RW_backend.Models
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
}
