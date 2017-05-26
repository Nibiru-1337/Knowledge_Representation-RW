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
    public static class ScenarioConsts
    {
        /*Tom i Bob grają w grę ruszając rękami.Jeśli obaj je uniosą bądź obaj je opuszczą, to zdobywają punkt.

MOVE by Tom causes TomRaised if ~TomRaised
MOVE by Tom causes ~TomRaised if TomRaised
MOVE by Bob causes BobRaised if ~BobRaised
MOVE by Bob causes ~BobRaised if BobRaised
always Point <=> (TomRaised <=> BobRaised)
*nonintertial Point
*MOVE by Tom releases ~Point

		*/

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
    }
    public class BaseWorldGenerator
    {
        public static Model GenerateWorld(bool withNoninertial, bool withReleases = false)
        {
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            Causes cause;
            List<Causes> causes = new List<Causes>();
            cause = new Causes(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive),
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Negated), ScenarioConsts.Move, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob));
            causes.Add(cause);

            cause = new Causes(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Negated),
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.BobRaised, FluentSign.Positive), ScenarioConsts.Move, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob));
            causes.Add(cause);

            cause = new Causes(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive),
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Negated), ScenarioConsts.Move, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom));
            causes.Add(cause);

            cause = new Causes(logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Negated),
                logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.TomRaised, FluentSign.Positive), ScenarioConsts.Move, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom));
            causes.Add(cause);

            HashSet<int> nonintertials = withNoninertial ? new HashSet<int>() {ScenarioConsts.Point} : new HashSet<int>() ;

            List<LogicClause> alwayses = new List<LogicClause>();
            AlternativeOfConjunctions always = new AlternativeOfConjunctions();
            UniformConjunction uc = UniformConjunction.CreateFrom(new List<int>() {ScenarioConsts.Point }, new List<int>() {ScenarioConsts.TomRaised, ScenarioConsts.BobRaised});
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() {ScenarioConsts.BobRaised }, new List<int>() {ScenarioConsts.TomRaised, ScenarioConsts.Point });
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() {ScenarioConsts.TomRaised }, new List<int>() {ScenarioConsts.BobRaised, ScenarioConsts.Point });
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() {ScenarioConsts.TomRaised, ScenarioConsts.BobRaised, ScenarioConsts.Point }, new List<int>());
            always.AddConjunction(uc);
            alwayses.Add(always);

            List<Releases> releases = withReleases 
                ? new List<Releases>()
                {
	                new Releases(new UniformAlternative(), ScenarioConsts.Point, ScenarioConsts.Move, 
						AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom))
                } 
                : new List<Releases>();


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
                ReleasesStatements = releases
            };

            return model;
        }
    }
}
