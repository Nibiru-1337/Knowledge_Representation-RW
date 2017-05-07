using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RW_Frontend.InputsViewModels
{
    public class CausesClauseViewModel
    {
        public string Action { get; set; }

        public List<string> Agents { get; set; }

        public string AlfaLogicExp { get; set; }

        public string PiLogicExp { get; set; }

        public CausesClauseViewModel(string action, List<string> agents, string alfaLogicexp, string piLogixExp)
        {
            this.Action = action;
            this.Agents = agents;
            this.AlfaLogicExp = alfaLogicexp;
            this.PiLogicExp = piLogixExp;
        }

        public static string GetActionFromView(StackPanel causesClauseStackPanel)
        {
            var actionComboBox = causesClauseStackPanel.Children[0] as ComboBox;
            if (actionComboBox == null)
                return null;
            return actionComboBox.SelectedItem as string;
        }

        public static List<string> GetAgentsFromView(StackPanel causesClauseStackPanel)
        {
            var agents = new List<string>();

            var expander = causesClauseStackPanel.Children[2] as Expander;
            if (expander == null)
                return null;
            var listBox = expander.Content as ListBox;
            if (listBox == null)
                return null;
            foreach (var agent in listBox.SelectedItems)
            {
                agents.Add(agent as string);
            }
            return agents;
        }

        public static string GetAlfaLogicExpFromView(StackPanel causesClauseStackPanel)
        {
            var alfaLogicExpTextBox = causesClauseStackPanel.Children[4] as TextBox;
            if (alfaLogicExpTextBox == null)
                return null;
            return alfaLogicExpTextBox.Text;
        }

        public static string GetPiLogicExpFromView(StackPanel causesClauseStackPanel)
        {
            var piLogicExpTextBox = causesClauseStackPanel.Children[6] as TextBox;
            if (piLogicExpTextBox == null)
                return null;
            return piLogicExpTextBox.Text;
        }
    }
}