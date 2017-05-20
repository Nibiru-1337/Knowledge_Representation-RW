using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RW_Frontend.InputsViewModels
{
    public class NoninertialClauseViewModel
    {
        public string Fluent { get; set; }

        public NoninertialClauseViewModel(string fluent)
        {
            this.Fluent = fluent;
        }

        public static string GetFluentFromView(StackPanel noninertialClauseStackPanel)
        {
            var fluentComboBox = noninertialClauseStackPanel.Children[1] as ComboBox;
            if (fluentComboBox == null)
                return null;
            return fluentComboBox.SelectedItem as string;
        }
    }
}