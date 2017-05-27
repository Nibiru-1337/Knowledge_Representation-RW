using System.Collections.Generic;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;

namespace RW_tests.UltimateSystemTests.InertialFluents
{
    public static class ScenarioConsts
    {
        /*"Zbliża się sesja i grupa znajomych postanowiła razem się pouczyć. Aby się czegoś nauczyć, musi być osoba, która dobrze zna dany temat. Bob i Tom są dobrzy z matematyki, a Alice z fizyki. Natomiast z fizyką u Toma jest różnie - czasem może kogoś jej nawet oduczyć. Alice ma młodszego brata, który często wpada niespodziewanie i jeśli nie ma zabawki, to nie można się przy nim uczyć, dlatego dziewczyna zawsze ma jakąś ze sobą.
Aby uczcić zakończenie nauki (albo przed na zachętę) czasem idą do baru się czegoś napić. Tom jest taką osobą, że jeśli pójdzie, to wszyscy się upiją, u Boba zależy to od nastroju. Jeśli się upiją, to zapominają to czego się nauczyli."							
LEARN by Tom causes Math
LEARN by Bob causes Math
LEARN by Alice causes Physics ^ HasToy
LEARN by Tom releases Physics
DRINK by Tom causes Drunk
DRINK by Bob releases Drunk if ~Drunk
always Drunk -> ~Physics ^ ~Math*/

        // agents
        public const int Bob = 0;
        public const int Tom = 1;
        public const int Alice = 2;
        public const int Jack = 3;
        // fluents
        public const int Physics = 0;
        public const int Math = 1;
        public const int Drunk = 2;
        public const int HasToy = 3;
        // actions
        public const int Learn = 0;
        public const int Drink = 1;
    }
    public class BaseWorldGenerator
    {
        public static Model GenerateWorld()
        {
            LogicClausesFactory logicClausesFactory = new LogicClausesFactory();
            Causes cause;
            List<Causes> causes = new List<Causes>();

            cause = new Causes(new UniformAlternative(), logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive),
                ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob));
            causes.Add(cause);

            cause = new Causes(new UniformAlternative(), logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Math, FluentSign.Positive),
                ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom));
            causes.Add(cause);

            UniformConjunction res = new UniformConjunction();
            res.AddFluent(ScenarioConsts.Physics, FluentSign.Positive);
            res.AddFluent(ScenarioConsts.HasToy, FluentSign.Positive);
            cause = new Causes(new UniformAlternative(), res,
                ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Alice));
            causes.Add(cause);
            
            cause = new Causes(new UniformAlternative(), logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.Drunk, FluentSign.Positive),
                ScenarioConsts.Drink, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom));
            causes.Add(cause);

            cause =
                Causes.CreateImpossible(
                    logicClausesFactory.CreateSingleFluentClause(ScenarioConsts.HasToy, FluentSign.Negated),
                    ScenarioConsts.Learn,
                    AgentsSet.CreateFromOneAgent(ScenarioConsts.Jack));
            causes.Add(cause);

            List<LogicClause> alwayses = new List<LogicClause>();
            AlternativeOfConjunctions always = new AlternativeOfConjunctions();
            UniformConjunction uc = UniformConjunction.CreateFrom(new List<int>(), new List<int>() { ScenarioConsts.Physics, ScenarioConsts.Math, ScenarioConsts.Drunk });
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() {ScenarioConsts.Drunk}, new List<int>() { ScenarioConsts.Physics, ScenarioConsts.Math});
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() { ScenarioConsts.Math }, new List<int>() { ScenarioConsts.Physics, ScenarioConsts.Drunk });
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() { ScenarioConsts.Physics }, new List<int>() { ScenarioConsts.Drunk, ScenarioConsts.Math });
            always.AddConjunction(uc);
            uc = UniformConjunction.CreateFrom(new List<int>() { ScenarioConsts.Physics, ScenarioConsts.Math }, new List<int>() { ScenarioConsts.Drunk });
            always.AddConjunction(uc);
            alwayses.Add(always);

            List<Releases> releases = new List<Releases>();
            Releases release = new Releases(new UniformAlternative(), ScenarioConsts.Physics, ScenarioConsts.Learn, AgentsSet.CreateFromOneAgent(ScenarioConsts.Tom));
            releases.Add(release);

            release = new Releases(new UniformAlternative(), ScenarioConsts.Drunk, ScenarioConsts.Drink, AgentsSet.CreateFromOneAgent(ScenarioConsts.Bob));
            releases.Add(release);


            var model = new Model
            {
                ActionsCount = 2,
                AgentsCount = 4,
                FluentsCount = 4,
                ActionsNames = new Dictionary<int, string>(),
                AgentsNames = new Dictionary<int, string>(),
                FluentsNames = new Dictionary<int, string>(),
                NoninertialFluents = new HashSet<int>(),
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
