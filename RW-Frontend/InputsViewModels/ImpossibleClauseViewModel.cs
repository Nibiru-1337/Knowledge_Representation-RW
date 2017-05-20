using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RW_Frontend.InputsViewModels
{
    public class ImpossibleClauseViewModel
    {
        public string Action { get; set; }

        public List<string> Agents { get; set; }

        public string PiLogicExp { get; set; }

        public ImpossibleClauseViewModel(string action, List<string> agents, string piLogixExp)
        {
            this.Action = action;
            this.Agents = agents;
            this.PiLogicExp = piLogixExp;
        }

        public static string GetActionFromView(StackPanel impossibleClauseStackPanel)
        {
            var actionComboBox = impossibleClauseStackPanel.Children[1] as ComboBox;
            if (actionComboBox == null)
                return null;
            return actionComboBox.SelectedItem as string;
        }

        public static List<string> GetAgentsFromView(StackPanel impossibleClauseStackPanel)
        {
            var agents = new List<string>();

            var expander = impossibleClauseStackPanel.Children[3] as Expander;
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

        public static string GetPiLogicExpFromView(StackPanel impossibleClauseStackPanel)
        {
            var piLogicExpTextBox = impossibleClauseStackPanel.Children[5] as TextBox;
            if (piLogicExpTextBox == null)
                return null;
            return piLogicExpTextBox.Text;
        }

    }
}
