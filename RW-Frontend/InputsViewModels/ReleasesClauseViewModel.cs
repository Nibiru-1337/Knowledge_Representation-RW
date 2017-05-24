using System.Collections.Generic;
using System.Windows.Controls;

namespace RW_Frontend.InputsViewModels
{
    public class ReleasesClauseViewModel
    {
        public string Action { get; set; }

        public List<string> Agents { get; set; }
        public string Fluent { get; set; }

        public string PiLogicExp { get; set; }

        public ReleasesClauseViewModel(string action, List<string> agents, string fluent, string piLogixExp)
        {
            Action = action;
            Agents = agents;
            Fluent = fluent;
            PiLogicExp = piLogixExp;
        }

        public static string GetActionFromView(StackPanel releasesClauseStackPanel)
        {
            var actionComboBox = releasesClauseStackPanel.Children[0] as ComboBox;
            if (actionComboBox == null)
                return null;
            return actionComboBox.SelectedItem as string;
        }

        public static List<string> GetAgentsFromView(StackPanel releasesClauseStackPanel)
        {
            var agents = new List<string>();

            var expander = releasesClauseStackPanel.Children[2] as Expander;
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

        public static string GetFluentFromView(StackPanel releasesClauseStackPanel)
        {
            var fluentComboBox = releasesClauseStackPanel.Children[4] as ComboBox;
            if (fluentComboBox == null)
                return null;
            return fluentComboBox.SelectedItem as string;
        }


        public static string GetPiLogicExpFromView(StackPanel releasesClauseStackPanel)
        {
            var piLogicExpTextBox = releasesClauseStackPanel.Children[6] as TextBox;
            if (piLogicExpTextBox == null)
                return null;
            return piLogicExpTextBox.Text;
        }
    }
}