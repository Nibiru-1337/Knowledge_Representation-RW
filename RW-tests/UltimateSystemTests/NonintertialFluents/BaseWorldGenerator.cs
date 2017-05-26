using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;

namespace RW_tests.UltimateSystemTests.NonintertialFluents
{
    public class BaseWorldGenerator
    {
        /*Tom i Bob grają w grę ruszając rękami.Jeśli obaj je uniosą bądź obaj je opuszczą, to zdobywają punkt.

MOVE by Tom causes TomRaised if ~TomRaised
MOVE by Tom causes ~TomRaised if TomRaised
MOVE by Bob causes BobRaised if ~BobRaised
MOVE by Bob causes ~BobRaised if BobRaised
always Point <=> (TomRaised <=> BobRaised)
nonintertial Point*/

        // agents
        public const int Bob = 0;
        public const int Tom = 1;
        // fluents
        public const int BobRaised = 0;
        public const int TomRaised = 1;
        public const int Point = 2;
        // actions
        public const int Move = 0;
        //// states
        //public const int TBP = 0; //TomRaised, BobRaised, Point
        //public const int NTNBP = 1; // ~TomRaised, ~BobRaised, Point
        //public const int TNBNP = 2; // TomRaised, ~BobRaised, ~Point
        //public const int NTBNP = 3; // ~TomRaised, BobRaised, ~Point
        public Model GenerateWorld(bool withNonintertial)
        {
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            Causes cause;
            List<Causes> causes = new List<Causes>();
            cause = new Causes(logicClausesFactory.CreateSingleFluentClause(BobRaised, FluentSign.Positive),
                logicClausesFactory.CreateSingleFluentClause(BobRaised, FluentSign.Negated), Move, AgentsSet.CreateFromOneAgent(Bob));
            causes.Add(cause);

            cause = new Causes(logicClausesFactory.CreateSingleFluentClause(BobRaised, FluentSign.Negated),
                logicClausesFactory.CreateSingleFluentClause(BobRaised, FluentSign.Positive), Move, AgentsSet.CreateFromOneAgent(Bob));
            causes.Add(cause);

            cause = new Causes(logicClausesFactory.CreateSingleFluentClause(TomRaised, FluentSign.Positive),
                logicClausesFactory.CreateSingleFluentClause(TomRaised, FluentSign.Negated), Move, AgentsSet.CreateFromOneAgent(Tom));
            causes.Add(cause);

            cause = new Causes(logicClausesFactory.CreateSingleFluentClause(TomRaised, FluentSign.Negated),
                logicClausesFactory.CreateSingleFluentClause(TomRaised, FluentSign.Positive), Move, AgentsSet.CreateFromOneAgent(Tom));
            causes.Add(cause);

            HashSet<int> nonintertials = withNonintertial ? new HashSet<int>() {Point} : new HashSet<int>() ;

            List<LogicClause> alwayses = new List<LogicClause>();
            AlternativeOfConjunctions always = new AlternativeOfConjunctions();
            UniformConjunction uc = UniformConjunction.CreateFrom(new List<int>() { Point }, new List<int>() {TomRaised, BobRaised});
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() { BobRaised }, new List<int>() { TomRaised, Point });
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() { TomRaised }, new List<int>() { BobRaised, Point });
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() { TomRaised, BobRaised, Point }, new List<int>());
            always.AddConjunction(uc);
            alwayses.Add(always);

            var model = new Model
            {
                ActionsCount = 1,
                AgentsCount = 2,
                FluentsCount = 3,
                ActionsNames = new Dictionary<int, string>(),
                AgentsNames = new Dictionary<int, string>(),
                FluentsNames = new Dictionary<int, string>(),
                NoninertialFluents = nonintertials,
                InitiallyStatements = new List<LogicClause>(),
                AlwaysStatements = alwayses,
                CausesStatements = causes,
                AfterStatements = new List<After>(),
                ReleasesStatements = new List<Releases>()
            };

            return model;
        }
    }
}
