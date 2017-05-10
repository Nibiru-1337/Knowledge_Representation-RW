using System;
using System.Windows;
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

        //TODO zapamiętywanie wyznaczonej reprezentacji świata

        public void CalculateAfterQuery(VM vm, AfterQueryViewModel afterQueryViewModel)
        {
            //var world = PrepareWorld(vm);
            var world = vm.World;
            var query = new ModelConverter().ConvertAfterQuery(afterQueryViewModel, InputAggregator.AgentsViewModels, InputAggregator.ActionsViewModels, InputAggregator.FluentsViewModels);

            var queryResult = query.Evaluate(world);

            MessageBox.Show(queryResult.IsTrue ? "Kwerenda spełniona" : "Kwerenda niespełniona", "Wynik kwerendy",
                MessageBoxButton.OK, MessageBoxImage.Information);
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
