using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RW_Frontend.InputsViewModels
{
    public static class InputAggregator
    {
        public static List<FluentViewModel> FluentsViewModels;
        public static List<ActionViewModel> ActionsViewModels;
        public static List<AgentViewModel> AgentsViewModels;
        public static List<CausesClauseViewModel> CausesClauseViewModels;
        public static List<AfterQueryViewModel> AfterQueriesViewModels;

        public static void PopulateViewModels(VM viewModel)
        {
            FluentsViewModels = PopulateFluents(viewModel);
            ActionsViewModels = PopulateActions(viewModel);
            AgentsViewModels = PopulateAgents(viewModel);

            CausesClauseViewModels = PopulateCausesClauses(viewModel);

            AfterQueriesViewModels = PopulateAfterQueries(viewModel);
        }

        private static List<FluentViewModel> PopulateFluents(VM viewModel)
        {
            var fluents = new List<FluentViewModel>();
            fluents.AddRange(viewModel.FluentsTextBoxes.Select(_ => new FluentViewModel(_.Text)));
            return fluents;
        }

        private static List<ActionViewModel> PopulateActions(VM viewModel)
        {
            var actions = new List<ActionViewModel>();
            actions.AddRange(viewModel.ActionsTextBoxes.Select(_ => new ActionViewModel(_.Text)));
            return actions;
        }

        private static List<AgentViewModel> PopulateAgents(VM viewModel)
        {
            var agents = new List<AgentViewModel>();
            agents.AddRange(viewModel.AgentsTextBoxes.Select(_ => new AgentViewModel(_.Text)));
            return agents;
        }

        private static List<CausesClauseViewModel> PopulateCausesClauses(VM viewModel)
        {
            var causesClauses = new List<CausesClauseViewModel>();
            foreach (var stackPanel in viewModel.CausesClausesStackPanels)
            {
                var action = CausesClauseViewModel.GetActionFromView(stackPanel);
                var agents = CausesClauseViewModel.GetAgentsFromView(stackPanel);
                var alfaLogicExp = CausesClauseViewModel.GetAlfaLogicExpFromView(stackPanel);
                var piLogicExp = CausesClauseViewModel.GetPiLogicExpFromView(stackPanel);

                causesClauses.Add(new CausesClauseViewModel(action, agents, alfaLogicExp, piLogicExp));
            }
            return causesClauses;
        }

        private static List<AfterQueryViewModel> PopulateAfterQueries(VM viewModel)
        {
            var afterQueries = new List<AfterQueryViewModel>();
            foreach (var stackPanel in viewModel.AfterQueryStackPanels)
            {
                var afterQueryType = AfterQueryViewModel.GetAfterQueryTypeFromView(stackPanel);
                var alfaLogicExp = AfterQueryViewModel.GetAlfaLogicExpFromView(stackPanel);
                var piLogicExp = AfterQueryViewModel.GetPiLogicExpFromView(stackPanel);
                var actionsByAgents = AfterQueryViewModel.GetActionsByAgentsFromView(stackPanel);

                afterQueries.Add(new AfterQueryViewModel(afterQueryType, alfaLogicExp, actionsByAgents, piLogicExp));
            }
            return afterQueries;
        }
    }
}