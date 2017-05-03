using System.Collections.Generic;

namespace RW_backend
{
    /// <summary>
    /// Reprezentacja posiadanej o świecie wiedzy - fluentów, akcji, agentów oraz wyrażeń
    /// </summary>
    public class Model
    {
        public int FluentsCount { get; set; }
        public int ActionsCount { get; set; }
        public int AgentsCount { get; set; }
        public IDictionary<int, string> FluentsNames{ get; set; }
        public IDictionary<int, string> ActionsNames{ get; set; }
        public IDictionary<int, string> AgentsNames{ get; set; }

        public ISet<int> NoninertialFluents { get; set; }

        public IList<LogicClause> AlwaysStatements { get; set; }
        public IList<LogicClause> InitiallyStatements { get; set; }

        public IList<After> AfterStatements { get; set; }
        public IList<Causes> CausesStatements { get; set; }
        public IList<Releases> ReleasesStatements { get; set; }
    }
    //TODO uzupełnić modele zdań
    /// <summary>
    /// Reprezentacja zdania "a after A1 by G1,...,An by Gn"
    /// </summary>
    public class After
    {

    }
    /// <summary>
    /// Reprezentacja zdania "A by G causes a if pi"
    /// </summary>
    public class Causes
    {

    }
    /// <summary>
    /// Reprezentacja zdania "A by G releases f"
    /// </summary>
    public class Releases
    {

    }
}
