using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RW_Frontend.InputsViewModels
{
    public class InitiallyClauseViewModel
    {
        public string AlfaLogicExp { get; set; }

        public InitiallyClauseViewModel(string alfaLogicexp)
        {
            this.AlfaLogicExp = alfaLogicexp;
        }

        public static string GetAlfaLogicExpFromView(StackPanel initiallyClauseStackPanel)
        {
            var alfaLogicExpTextBox = initiallyClauseStackPanel.Children[1] as TextBox;
            if (alfaLogicExpTextBox == null)
                return null;
            return alfaLogicExpTextBox.Text;
        }
    }
}
