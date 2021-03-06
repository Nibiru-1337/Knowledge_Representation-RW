﻿using System.Collections.Generic;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;

namespace RW_backend.Models
{
    /// <summary>
    /// Reprezentacja posiadanej o świecie wiedzy - fluentów, akcji, agentów oraz wyrażeń
    /// to jest to przejściowe między frontendem a backendem
    /// czyli: frontend tworzy sobie Model
    /// my na podstawie Modelu tworzymy World
    /// </summary>
    public class Model
    {
        public int FluentsCount { get; set; }
        public int ActionsCount { get; set; }
        public int AgentsCount { get; set; }

        //TODO: nie lepiej zwykłe tablice stringów?
        // a słowniki string->int?
        public IDictionary<int, string> FluentsNames { get; set; }
        public IDictionary<int, string> ActionsNames { get; set; }
        public IDictionary<int, string> AgentsNames { get; set; }

        public ISet<int> NoninertialFluents { get; set; }

        public IList<LogicClause> AlwaysStatements { get; set; }
        public IList<LogicClause> InitiallyStatements { get; set; }

        public IList<After> AfterStatements { get; set; }
        public IList<Causes> CausesStatements { get; set; }
        public IList<Releases> ReleasesStatements { get; set; }
    }
}
