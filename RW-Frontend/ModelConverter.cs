using System.Collections.Generic;
using System.Linq;
using RW_backend.Logic.Parser;
using RW_backend.Logic.Queries;
using RW_backend.Models;
using RW_backend.Models.BitSets;
using RW_backend.Models.Clauses;
using RW_backend.Models.Clauses.LogicClauses;
using RW_backend.Models.Factories;
using RW_backend.Models.GraphModels;
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

            return ConvertToModel(InputAggregator.FluentsViewModels, InputAggregator.ActionsViewModels, InputAggregator.AgentsViewModels, InputAggregator.CausesClauseViewModels);
        }

        internal Model ConvertToModel(List<FluentViewModel> fluentsViewModels, List<ActionViewModel> actionsViewModels, List<AgentViewModel> agentsViewModels,
            List<CausesClauseViewModel> causesClauseViewModels)
        {
            var model = new Model();
            var reverseFluentDict = ConvertFluents(model, fluentsViewModels);
            var reverseActionDict = ConvertActions(model, actionsViewModels);
            var reverseAgentDict = ConvertAgents(model, agentsViewModels);

            var parser = new Parser(reverseFluentDict);

            ConvertCauses(reverseAgentDict, parser, reverseActionDict, model, causesClauseViewModels);

            return model;
        }

        private static void ConvertCauses(Dictionary<string, int> revAgentDict, Parser parser, Dictionary<string, int> revActionDict, Model model, List<CausesClauseViewModel> causesVms)
        {
            var causesList = new List<Causes>();
            foreach (var t in causesVms)
            {
                AgentsSet agentSet;
                if (t.Agents.Contains(VM.AnyAgent))
                {
                    agentSet = new AgentsSet(0);
                }
                else
                {
                    List<int> agentIds = t.Agents.Select(a => revAgentDict[a]).ToList();
                    BitSetFactory bitSetFactory = new BitSetFactory();
                    int set = bitSetFactory.CreateBitSetValueFrom(agentIds);
                    agentSet = new AgentsSet(set);
                }
                LogicClause effect = parser.ParseToLogicClause(t.AlfaLogicExp);
                LogicClause condition = parser.ParseToLogicClause(t.PiLogicExp);
                int actionId = revActionDict[t.Action];
                causesList.Add(new Causes(condition, effect, actionId, agentSet));
            }
            model.CausesStatements = causesList;
        }

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
            for (int i = 0; i < agents.Count; i++)
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
            for (int i = 0; i < actions.Count; i++)
            {
                actionDict.Add(i, actions[i].Action);
                revActionDict.Add(actions[i].Action, i);
            }
            return revActionDict;
        }

        private static Dictionary<string, int> ConvertFluents(Model model, List<FluentViewModel> fluents)
        {
            Dictionary<int, string> fluentDict;
            Dictionary<string, int> revFluentDict;
            revFluentDict = GetFluentDictionaries(fluents, out fluentDict);
            model.FluentsCount = fluents.Count;
            model.FluentsNames = fluentDict;
            return revFluentDict;
        }

        private static Dictionary<string, int> GetFluentDictionaries(List<FluentViewModel> fluents, out Dictionary<int, string> fluentDict)
        {
            fluentDict = new Dictionary<int, string>();
            var revFluentDict = new Dictionary<string, int>();
            for (int i = 0; i < fluents.Count; i++)
            {
                fluentDict.Add(i, fluents[i].Fluent);
                revFluentDict.Add(fluents[i].Fluent, i);
            }
            return revFluentDict;
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
            var program = new List<ActionAgentsPair>();
            foreach (var actionBy in scenario)
            {
                string actionName = actionBy.Item1;
                int actionId = revActionDict[actionName];
                List<string> agentNames = actionBy.Item2;
                List<int> agentIds = agentNames.Select(a => revAgentDict[a]).ToList();
                BitSetFactory bitSetFactory = new BitSetFactory();
                int set = bitSetFactory.CreateBitSetValueFrom(agentIds);
                program.Add(new ActionAgentsPair(actionId, set));
            }

            var parser = new Parser(revFluentDict);

            var alfa = parser.ParseToLogicClause(alfaLogicExp);
            var pi = parser.ParseToLogicClause(piLogicExp);

            var always = type == AfterQueryViewModel.AfterQueryNecOrPos.Necessary;
            var afterQuery = new AfterQuery(program, pi, always, alfa);

            return afterQuery;
        }

        /// <summary>
        /// Zamień obiekt Modelu na równoważny obiekt ViewModel
        /// </summary>
        /// <param name="model">obiekt Modelu</param>
        /// <returns>obiekt ViewModel</returns>
        public VM ConvertToVM(Model model)
        {
            //TODO konwersja M-VM (? - o ile będzie potrzebna)
            return new VM();
        }
    }
}
