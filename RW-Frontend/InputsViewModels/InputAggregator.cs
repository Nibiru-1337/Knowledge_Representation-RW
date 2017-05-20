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

        public static List<InitiallyClauseViewModel> InitiallyClauseViewModels;
        public static List<AfterClauseViewModel> AfterClauseViewModels;
        public static List<ObservableClauseViewModel> ObservableClauseViewModels;
        public static List<CausesClauseViewModel> CausesClauseViewModels;
        public static List<ImpossibleClauseViewModel> ImpossibleClauseViewModels;
        public static List<ReleasesClauseViewModel> ReleasesClauseViewModels;
        public static List<AlwaysClauseViewModel> AlwaysClauseViewModels;
        public static List<NoninertialClauseViewModel> NoninertialClauseViewModels;

        public static List<ExecutableQueryViewModel> ExecutableQueriesViewModels;
        public static List<AfterQueryViewModel> AfterQueriesViewModels;
        public static List<EngagedQueryViewModel> EngagedQueriesViewModels;

        public static void PopulateViewModels(VM viewModel)
        {
            FluentsViewModels = PopulateFluents(viewModel);
            ActionsViewModels = PopulateActions(viewModel);
            AgentsViewModels = PopulateAgents(viewModel);

            InitiallyClauseViewModels = PopulateInitiallyClauses(viewModel);
            AfterClauseViewModels = PopulateAfterClauses(viewModel);
            ObservableClauseViewModels = PopulateObservableClauses(viewModel);
            CausesClauseViewModels = PopulateCausesClauses(viewModel);
            ImpossibleClauseViewModels = PopulateImpossibleClauses(viewModel);
            ReleasesClauseViewModels = PopulateReleasesClauses(viewModel);
            AlwaysClauseViewModels = PopulateAlwaysClauses(viewModel);
            NoninertialClauseViewModels = PopulateNoninertialClauses(viewModel);

            ExecutableQueriesViewModels = PopulateExecutableQueries(viewModel);
            AfterQueriesViewModels = PopulateAfterQueries(viewModel);
            EngagedQueriesViewModels = PopulateEngagedQueries(viewModel);
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

        private static List<InitiallyClauseViewModel> PopulateInitiallyClauses(VM viewModel)
        {
            var initiallyClauses = new List<InitiallyClauseViewModel>();
            foreach (var stackPanel in viewModel.InitiallyClausesStackPanels)
            {
                var alfaLogicExp = InitiallyClauseViewModel.GetAlfaLogicExpFromView(stackPanel);
                initiallyClauses.Add(new InitiallyClauseViewModel(alfaLogicExp));
            }
            return initiallyClauses;
        }

        private static List<AfterClauseViewModel> PopulateAfterClauses(VM viewModel)
        {
            var afterClauses = new List<AfterClauseViewModel>();
            foreach (var stackPanel in viewModel.AfterClausesStackPanels)
            {
                var alfaLogicExp = AfterClauseViewModel.GetAlfaLogicExpFromView(stackPanel);
                var actionsByAgents = AfterClauseViewModel.GetActionsByAgentsFromView(stackPanel);
                afterClauses.Add(new AfterClauseViewModel(alfaLogicExp, actionsByAgents));
            }
            return afterClauses;
        }

        private static List<ObservableClauseViewModel> PopulateObservableClauses(VM viewModel)
        {
            var observableClauses = new List<ObservableClauseViewModel>();
            foreach (var stackPanel in viewModel.ObservableClausesStackPanels)
            {
                var alfaLogicExp = ObservableClauseViewModel.GetAlfaLogicExpFromView(stackPanel);
                var actionsByAgents = ObservableClauseViewModel.GetActionsByAgentsFromView(stackPanel);
                observableClauses.Add(new ObservableClauseViewModel(alfaLogicExp, actionsByAgents));
            }
            return observableClauses;
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

        private static List<ImpossibleClauseViewModel> PopulateImpossibleClauses(VM viewModel)
        {
            var impossibleClauses = new List<ImpossibleClauseViewModel>();
            foreach (var stackPanel in viewModel.ImpossibleClausesStackPanels)
            {
                var action = ImpossibleClauseViewModel.GetActionFromView(stackPanel);
                var agents = ImpossibleClauseViewModel.GetAgentsFromView(stackPanel);
                var piLogicExp = ImpossibleClauseViewModel.GetPiLogicExpFromView(stackPanel);

                impossibleClauses.Add(new ImpossibleClauseViewModel(action, agents, piLogicExp));
            }
            return impossibleClauses;
        }

        private static List<ReleasesClauseViewModel> PopulateReleasesClauses(VM viewModel)
        {
            var releasesClauses = new List<ReleasesClauseViewModel>();
            foreach (var stackPanel in viewModel.ReleasesClausesStackPanels)
            {
                var action = ReleasesClauseViewModel.GetActionFromView(stackPanel);
                var agents = ReleasesClauseViewModel.GetAgentsFromView(stackPanel);
                var fluent = ReleasesClauseViewModel.GetFluentFromView(stackPanel);
                var piLogicExp = ReleasesClauseViewModel.GetPiLogicExpFromView(stackPanel);

                releasesClauses.Add(new ReleasesClauseViewModel(action, agents, fluent, piLogicExp));
            }
            return releasesClauses;
        }

        private static List<AlwaysClauseViewModel> PopulateAlwaysClauses(VM viewModel)
        {
            var alwaysClauses = new List<AlwaysClauseViewModel>();
            foreach (var stackPanel in viewModel.AlwaysClausesStackPanels)
            {
                var alfaLogicExp = AlwaysClauseViewModel.GetAlfaLogicExpFromView(stackPanel);
                alwaysClauses.Add(new AlwaysClauseViewModel(alfaLogicExp));
            }
            return alwaysClauses;
        }

        private static List<NoninertialClauseViewModel> PopulateNoninertialClauses(VM viewModel)
        {
            var noninertialClauses = new List<NoninertialClauseViewModel>();
            foreach (var stackPanel in viewModel.NoninertialClausesStackPanels)
            {
                var fluent = NoninertialClauseViewModel.GetFluentFromView(stackPanel);
                noninertialClauses.Add(new NoninertialClauseViewModel(fluent));
            }
            return noninertialClauses;
        }

        private static List<ExecutableQueryViewModel> PopulateExecutableQueries(VM viewModel)
        {
            var executableQueries = new List<ExecutableQueryViewModel>();
            foreach (var stackPanel in viewModel.ExecutableQueryStackPanels)
            {
                var executableQueryType = ExecutableQueryViewModel.GetExecutableQueryTypeFromView(stackPanel);
                var actionsByAgents = ExecutableQueryViewModel.GetActionsByAgentsFromView(stackPanel);
                var piLogicExp = ExecutableQueryViewModel.GetPiLogicExpFromView(stackPanel);

                executableQueries.Add(new ExecutableQueryViewModel(executableQueryType, actionsByAgents, piLogicExp));
            }
            return executableQueries;
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

        private static List<EngagedQueryViewModel> PopulateEngagedQueries(VM viewModel)
        {
            var engagedQueries = new List<EngagedQueryViewModel>();
            foreach (var stackPanel in viewModel.EngagedQueryStackPanels)
            {
                var agents = EngagedQueryViewModel.GetAgentsFromView(stackPanel);
                var engagedQueryType = EngagedQueryViewModel.GetEngagedQueryTypeFromView(stackPanel);
                var actionsByAgents = EngagedQueryViewModel.GetActionsByAgentsFromView(stackPanel);
                var piLogicExp = EngagedQueryViewModel.GetPiLogicExpFromView(stackPanel);

                engagedQueries.Add(new EngagedQueryViewModel(agents, engagedQueryType, actionsByAgents, piLogicExp));
            }
            return engagedQueries;
        }
    }
}