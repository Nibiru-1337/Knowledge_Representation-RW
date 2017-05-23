using System;
using System.Windows;
using System.Windows.Controls;
using RW_backend.Logic;
using RW_backend.Models.World;
using RW_Frontend.InputsViewModels;

namespace RW_Frontend
{
    /// <summary>
    /// Logika odpowiedzialna za interakcje z użytkownikiem i obsługę widoku
    /// </summary>
    class FrontendLogic
    {
        public void SetDataContext(MainWindow mainWindow)
        {
            mainWindow.DataContext = VM.Create();
        }

        public void CalculateExecutableQuery(VM vm, ExecutableQueryViewModel executableQueryViewModel, StackPanel executableQueryStackPanel)
        {
            var world = vm.World;
            var query = new ModelConverter().ConvertExecutableQuery(executableQueryViewModel, InputAggregator.AgentsViewModels, InputAggregator.ActionsViewModels, InputAggregator.FluentsViewModels);

            var queryResult = query.Evaluate(world);

            executableQueryViewModel.SetResultLabel(executableQueryStackPanel, queryResult.IsTrue);
            GC.Collect();
        }

        public void CalculateAfterQuery(VM vm, AfterQueryViewModel afterQueryViewModel, StackPanel afterQueryStackPanel)
        {
            var world = vm.World;
            var query = new ModelConverter().ConvertAfterQuery(afterQueryViewModel, InputAggregator.AgentsViewModels, InputAggregator.ActionsViewModels, InputAggregator.FluentsViewModels);

            var queryResult = query.Evaluate(world);

            afterQueryViewModel.SetResultLabel(afterQueryStackPanel, queryResult.IsTrue);
            GC.Collect();
        }

        public void CalculateEngagedQuery(VM vm, EngagedQueryViewModel engagedQueryViewModel, StackPanel engagedQueryStackPanel)
        {
            var world = vm.World;
            var query = new ModelConverter().ConvertEngagedQuery(engagedQueryViewModel, InputAggregator.AgentsViewModels, InputAggregator.ActionsViewModels, InputAggregator.FluentsViewModels);

            var queryResult = query.Evaluate(world);

            engagedQueryViewModel.SetResultLabel(engagedQueryStackPanel, queryResult.IsTrue);
            GC.Collect();
        }

        public World PrepareWorld(VM vm)
        {
            var model = new ModelConverter().ConvertToModel(vm);

            var world = new BackendLogic().CalculateWorld(model);

            return world;
        }
    }
}
