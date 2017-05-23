using System.Windows.Controls;

namespace RW_Frontend.InputsViewModels
{
    public class AlwaysClauseViewModel
    {
        public string AlfaLogicExp { get; set; }

        public AlwaysClauseViewModel(string alfaLogicexp)
        {
            AlfaLogicExp = alfaLogicexp;
        }

        public static string GetAlfaLogicExpFromView(StackPanel alwaysClauseStackPanel)
        {
            var alfaLogicExpTextBox = alwaysClauseStackPanel.Children[1] as TextBox;
            if (alfaLogicExpTextBox == null)
                return null;
            return alfaLogicExpTextBox.Text;
        }
    }
}