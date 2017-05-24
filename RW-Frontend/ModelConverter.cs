using System;
using System.Collections.Generic;
using System.Linq;
using RW_backend.Logic.Parser;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.World;
using RW_Frontend.InputsViewModels;

namespace RW_Frontend
{
    /// <summary>
    /// Konwerter Model-ViewModel
    /// </summary>
    class ModelConverter
    {
        /// <summary>
        /// Zamień obiekt VM na równoważny obiekt Modelu
        /// </summary>
        /// <param name="vm">obiekt ViewModel</param>
        /// <returns>obiekt Modelu</returns>
        public Model ConvertToModel(VM vm)
        {
            //TODO konwersja VM-M - przekazywanie przez statyczne właściwości wygląda źle
            InputAggregator.PopulateViewModels(vm);

            return ConvertToModel(InputAggregator.FluentsViewModels, InputAggregator.ActionsViewModels, InputAggregator.AgentsViewModels, InputAggregator.CausesClauseViewModels,
                InputAggregator.InitiallyClauseViewModels, InputAggregator.AfterClauseViewModels, InputAggregator.ObservableClauseViewModels,
                InputAggregator.ImpossibleClauseViewModels, InputAggregator.ReleasesClauseViewModels, InputAggregator.AlwaysClauseViewModels, InputAggregator.NoninertialClauseViewModels);
        }

        internal Model ConvertToModel(List<FluentViewModel> fluentsViewModels, List<ActionViewModel> actionsViewModels, List<AgentViewModel> agentsViewModels,
            List<CausesClauseViewModel> causesClauseViewModels, List<InitiallyClauseViewModel> initiallyClauseViewModels, List<AfterClauseViewModel> afterClauseViewModels, List<ObservableClauseViewModel> observableClauseViewModels, List<ImpossibleClauseViewModel> impossibleClauseViewModels, List<ReleasesClauseViewModel> releasesClauseViewModels, List<AlwaysClauseViewModel> alwaysClauseViewModels, List<NoninertialClauseViewModel> noninertialClauseViewModels)
        {
            var model = new Model();
            var reverseFluentDict = ConvertFluents(model, fluentsViewModels);
            var reverseActionDict = ConvertActions(model, actionsViewModels);
            var reverseAgentDict = ConvertAgents(model, agentsViewModels);

            var parser = new Parser(reverseFluentDict);

            model.CausesStatements = ConvertCauses(parser, causesClauseViewModels, impossibleClauseViewModels, reverseActionDict, reverseAgentDict);
            model.InitiallyStatements = ConvertInitially(parser, initiallyClauseViewModels);
            model.AfterStatements = ConvertAfterStatements(parser, afterClauseViewModels, observableClauseViewModels, reverseActionDict, reverseAgentDict);
            model.ReleasesStatements = ConvertReleases(parser, releasesClauseViewModels, reverseFluentDict, reverseActionDict, reverseAgentDict);
            model.AlwaysStatements = ConvertAlways(parser, alwaysClauseViewModels);
            model.NoninertialFluents = ConvertNoninertial(noninertialClauseViewModels, reverseFluentDict);
            return model;
        }

        #region Convert causes

        private static List<Causes> ConvertCauses(Parser parser, List<CausesClauseViewModel> causesVms, List<ImpossibleClauseViewModel> impossibleVms, Dictionary<string, int> revActionDict, Dictionary<string, int> revAgentDict)
        {
            var causes = ConvertCauses(parser, causesVms, revActionDict, revAgentDict);
            var impossible = ConvertImpossible(parser, impossibleVms, revActionDict, revAgentDict);
            return causes.Concat(impossible).ToList();
        }

        private static List<Causes> ConvertCauses(Parser parser, List<CausesClauseViewModel> causesVms, Dictionary<string, int> revActionDict, Dictionary<string, int> revAgentDict)
        {
            var causesList = new List<Causes>();
            if (causesVms == null) return causesList;
            foreach (var t in causesVms)
            {
                var agentSet = ConvertAgentsSet(revAgentDict, t.Agents);
                var effect = parser.ParseToLogicClause(t.AlfaLogicExp);
                var condition = parser.ParseToLogicClause(t.PiLogicExp);
                var actionId = revActionDict[t.Action];
                causesList.Add(new Causes(condition, effect, actionId, agentSet));
            }
            return causesList;
        }

        private static List<Causes> ConvertImpossible(Parser parser, List<ImpossibleClauseViewModel> impossibleVms, Dictionary<string, int> revActionDict, Dictionary<string, int> revAgentDict)
        {
            var causesList = new List<Causes>();
            if (impossibleVms == null) return causesList;
            var factory = new LogicClausesFactory();
            foreach (var t in impossibleVms)
            {
                var agentSet = ConvertAgentsSet(revAgentDict, t.Agents);
                var effect = factory.CreateContradictingClause();
                var condition = parser.ParseToLogicClause(t.PiLogicExp);
                var actionId = revActionDict[t.Action];
                causesList.Add(new Causes(condition, effect, actionId, agentSet));
            }
            return causesList;
        }

        #endregion

        private static List<LogicClause> ConvertInitially(Parser parser, List<InitiallyClauseViewModel> initiallyVms)
        {
            if (initiallyVms == null) return new List<LogicClause>();
            return initiallyVms.Select(t => parser.ParseToLogicClause(t.AlfaLogicExp)).ToList();
        }

        #region Convert after

        private IList<After> ConvertAfterStatements(Parser parser, List<AfterClauseViewModel> afterVms, List<ObservableClauseViewModel> observableVms, Dictionary<string, int> reverseActionDict, Dictionary<string, int> reverseAgentDict)
        {
            var after = ConvertAfterStatements(parser, afterVms, reverseActionDict, reverseAgentDict);
            var observable = ConvertObservableStatements(parser, observableVms, reverseActionDict, reverseAgentDict);
            return after.Concat(observable).ToList();
        }

        private IList<After> ConvertAfterStatements(Parser parser, List<AfterClauseViewModel> afterVms, Dictionary<string, int> reverseActionDict, Dictionary<string, int> reverseAgentDict)
        {
            var afterList = new List<After>();
            if (afterVms == null) return afterList;
            foreach (var afterClauseViewModel in afterVms)
            {
                var effect = parser.ParseToLogicClause(afterClauseViewModel.AlfaLogicExp);

                IReadOnlyList<ActionAgentsPair> program = ConvertScenario(afterClauseViewModel.ActionByAgents, reverseActionDict, reverseAgentDict);
                afterList.Add(new After(effect, program, true));
            }

            return afterList;
        }

        private IList<After> ConvertObservableStatements(Parser parser, List<ObservableClauseViewModel> observableVms, Dictionary<string, int> reverseActionDict, Dictionary<string, int> reverseAgentDict)
        {
            var afterList = new List<After>();
            if (observableVms == null) return afterList;
            foreach (var afterClauseViewModel in observableVms)
            {
                var effect = parser.ParseToLogicClause(afterClauseViewModel.AlfaLogicExp);

                IReadOnlyList<ActionAgentsPair> program = ConvertScenario(afterClauseViewModel.ActionByAgents, reverseActionDict, reverseAgentDict);
                afterList.Add(new After(effect, program, false));
            }

            return afterList;
        }

        #endregion

        private static IList<Releases> ConvertReleases(Parser parser, List<ReleasesClauseViewModel> releasesClauseViewModels, Dictionary<string, int> reverseFluentDict, Dictionary<string, int> reverseActionDict, Dictionary<string, int> reverseAgentDict)
        {
            var releasesList = new List<Releases>();
            if (releasesClauseViewModels == null) return releasesList;
            foreach (var releasesClauseViewModel in releasesClauseViewModels)
            {
                var initialCondition = parser.ParseToLogicClause(releasesClauseViewModel.PiLogicExp);
                var fluentReleased = reverseFluentDict[releasesClauseViewModel.Fluent];
                var action = reverseActionDict[releasesClauseViewModel.Action];
                var agentsSet = ConvertAgentsSet(reverseAgentDict, releasesClauseViewModel.Agents);
                releasesList.Add(new Releases(initialCondition, fluentReleased, action, agentsSet));
            }
            return releasesList;
        }

        private static IList<LogicClause> ConvertAlways(Parser parser, List<AlwaysClauseViewModel> alwaysClauseViewModels)
        {
            if (alwaysClauseViewModels == null) return new List<LogicClause>();
            return alwaysClauseViewModels.Select(t => parser.ParseToLogicClause(t.AlfaLogicExp)).ToList();
        }

        private static ISet<int> ConvertNoninertial(List<NoninertialClauseViewModel> noninertialClauseViewModels, IReadOnlyDictionary<string, int> reverseFluentDict)
        {
            var set = new HashSet<int>();
            if (noninertialClauseViewModels == null) return set;
            foreach (var noninertialClauseViewModel in noninertialClauseViewModels)
            {
                var fluent = reverseFluentDict[noninertialClauseViewModel.Fluent];
                set.Add(fluent);
            }
            return set;
        }

        #region Convert common elements
        private List<ActionAgentsPair> ConvertScenario(List<Tuple<string, List<string>>> actionsByAgents, IReadOnlyDictionary<string, int> reverseActionDict, Dictionary<string, int> reverseAgentDict)
        {
            var actions = new List<ActionAgentsPair>();
            foreach (var actionByAgent in actionsByAgents)
            {
                var actionId = reverseActionDict[actionByAgent.Item1];
                var agentSet = ConvertAgentsSet(reverseAgentDict, actionByAgent.Item2);
                actions.Add(new ActionAgentsPair(actionId, agentSet.AgentSet));
            }
            return actions;
        }

        private static AgentsSet ConvertAgentsSet(IReadOnlyDictionary<string, int> revAgentDict, List<string> agents)
        {
            AgentsSet agentSet;
            if (agents.Contains(VM.AnyAgent))
            {
                agentSet = new AgentsSet(0);
            }
            else
            {
                var agentIds = agents.Select(a => revAgentDict[a]).ToList();
                var bitSetFactory = new BitSetFactory();
                var set = bitSetFactory.CreateBitSetValueFrom(agentIds);
                agentSet = new AgentsSet(set);
            }
            return agentSet;
        }

        #endregion

        #region Convert base elements - fluents, actions, agents

        private static Dictionary<string, int> ConvertAgents(Model model, List<AgentViewModel> agents)
        {
            Dictionary<int, string> agentDict;
            var revAgentDict = GetAgentDictionaries(agents, out agentDict);
            model.AgentsCount = agents.Count;
            model.AgentsNames = agentDict;
            return revAgentDict;
        }

        private static Dictionary<string, int> GetAgentDictionaries(List<AgentViewModel> agents, out Dictionary<int, string> agentDict)
        {
            agentDict = new Dictionary<int, string>();
            var revAgentDict = new Dictionary<string, int>();
            for (var i = 0; i < agents.Count; i++)
            {
                agentDict.Add(i, agents[i].Agent);
                revAgentDict.Add(agents[i].Agent, i);
            }
            return revAgentDict;
        }

        private static Dictionary<string, int> ConvertActions(Model model, List<ActionViewModel> actions)
        {
            Dictionary<int, string> actionDict;
            var revActionDict = GetActionDictionaries(actions, out actionDict);
            model.ActionsCount = actions.Count;
            model.ActionsNames = actionDict;
            return revActionDict;
        }

        private static Dictionary<string, int> GetActionDictionaries(List<ActionViewModel> actions, out Dictionary<int, string> actionDict)
        {
            actionDict = new Dictionary<int, string>();
            var revActionDict = new Dictionary<string, int>();
            for (var i = 0; i < actions.Count; i++)
            {
                actionDict.Add(i, actions[i].Action);
                revActionDict.Add(actions[i].Action, i);
            }
            return revActionDict;
        }

        private static Dictionary<string, int> ConvertFluents(Model model, List<FluentViewModel> fluents)
        {
            Dictionary<int, string> fluentDict;
            var revFluentDict = GetFluentDictionaries(fluents, out fluentDict);
            model.FluentsCount = fluents.Count;
            model.FluentsNames = fluentDict;
            return revFluentDict;
        }

        private static Dictionary<string, int> GetFluentDictionaries(List<FluentViewModel> fluents, out Dictionary<int, string> fluentDict)
        {
            fluentDict = new Dictionary<int, string>();
            var revFluentDict = new Dictionary<string, int>();
            for (var i = 0; i < fluents.Count; i++)
            {
                fluentDict.Add(i, fluents[i].Fluent);
                revFluentDict.Add(fluents[i].Fluent, i);
            }
            return revFluentDict;
        }

        #endregion

        #region Convert queries

        public ExecutableQuery ConvertExecutableQuery(ExecutableQueryViewModel viewModel, List<AgentViewModel> agentsViewModels, List<ActionViewModel> actionsViewModels, List<FluentViewModel> fluentsViewModels)
        {
            var scenario = viewModel.ActionByAgents;
            var queryType = viewModel.ExecutableQueryType;

            var piLogicExp = viewModel.PiLogicExp;

            Dictionary<int, string> ign;
            var revAgentDict = GetAgentDictionaries(agentsViewModels, out ign);
            var revActionDict = GetActionDictionaries(actionsViewModels, out ign);
            var revFluentDict = GetFluentDictionaries(fluentsViewModels, out ign);
            var program = ConvertScenario(scenario, revActionDict, revAgentDict);

            var parser = new Parser(revFluentDict);

            var initialState = parser.ParseToLogicClause(piLogicExp);

            var always = queryType == ExecutableQueryViewModel.ExecutableQueryAlwaysOrNot.Always;

            return new ExecutableQuery(program, initialState, always);
        }

        public AfterQuery ConvertAfterQuery(AfterQueryViewModel viewModel, List<AgentViewModel> agentsViewModels, List<ActionViewModel> actionsViewModels, List<FluentViewModel> fluentsViewModels)
        {
            var scenario = viewModel.ActionByAgents;
            var type = viewModel.AfterQueryType;

            var alfaLogicExp = viewModel.AlfaLogicExp;
            var piLogicExp = viewModel.PiLogicExp;

            Dictionary<int, string> ign;
            var revAgentDict = GetAgentDictionaries(agentsViewModels, out ign);
            var revActionDict = GetActionDictionaries(actionsViewModels, out ign);
            var revFluentDict = GetFluentDictionaries(fluentsViewModels, out ign);
            var program = ConvertScenario(scenario, revActionDict, revAgentDict);

            var parser = new Parser(revFluentDict);

            var alfa = parser.ParseToLogicClause(alfaLogicExp);
            var pi = parser.ParseToLogicClause(piLogicExp);

            var always = type == AfterQueryViewModel.AfterQueryNecOrPos.Necessary;
            var afterQuery = new AfterQuery(program, pi, always, alfa);

            return afterQuery;
        }

        public EngagedQuery ConvertEngagedQuery(EngagedQueryViewModel viewModel, List<AgentViewModel> agentsViewModels, List<ActionViewModel> actionsViewModels, List<FluentViewModel> fluentsViewModels)
        {

            var scenario = viewModel.ActionByAgents;
            var queryType = viewModel.EngagedQueryType;
            var agents = viewModel.Agents;
            var piLogicExp = viewModel.PiLogicExp;

            Dictionary<int, string> ign;
            var revAgentDict = GetAgentDictionaries(agentsViewModels, out ign);
            var revActionDict = GetActionDictionaries(actionsViewModels, out ign);
            var revFluentDict = GetFluentDictionaries(fluentsViewModels, out ign);
            var program = ConvertScenario(scenario, revActionDict, revAgentDict);

            var parser = new Parser(revFluentDict);
            var initialState = parser.ParseToLogicClause(piLogicExp);

            var always = EngagedQueryViewModel.EngagedQueryAlwaysOrNot.Always == queryType;

            var agentsSet = ConvertAgentsSet(revAgentDict, agents);

            return new EngagedQuery(program, initialState, always, agentsSet);
        }

        #endregion
    }
}
