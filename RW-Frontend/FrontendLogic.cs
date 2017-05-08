using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using RW_backend.Logic;
using RW_backend.Logic.Queries.Results;
using RW_backend.Models;
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

        //TODO obsługiwanie modyfikacji vm - dodawanie/usuwanie elementów
        //coś na kształt poniższych, ale należy też sprawdzać poprawność - usuwanie używanych fluentów/poprawność formuł w zdaniach; docelowo dla każdego rodzaju wpisów pewnie będzie inaczej
        public void AddItem<T>(ObservableCollection<T> collection) where T : new()
        {
            collection.Add(new T());
        }

        public void RemoveItem<T>(ObservableCollection<T> collection, T item)
        {
            collection.Remove(item);
        }

        //TODO zapamiętywanie wyznaczonej reprezentacji świata
        //TODO wywoływanie obliczeń kwerend z BackendLogic

        public void CalculateAfterQuery(VM vm, AfterQueryViewModel afterQueryViewModel)
        {
            var world = PrepareWorld(vm);
            var query = new ModelConverter().ConvertAfterQuery(afterQueryViewModel, InputAggregator.AgentsViewModels,InputAggregator.ActionsViewModels,InputAggregator.FluentsViewModels);

            var queryResult = query.Evaluate(world);

            MessageBox.Show(queryResult.IsTrue ? "Kwerenda spełniona" : "Kwerenda niespełniona", "Wynik kwerendy",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        public World PrepareWorld(VM vm)
        {
            var model = new ModelConverter().ConvertToModel(vm);

            var world = new BackendLogic().CalculateWorld(model);

            return world;
        }
    }
}
