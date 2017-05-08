using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RW_Frontend.InputsViewModels
{
    public class AfterQueryViewModel
    {
        public AfterQueryNecOrPos AfterQueryType { get; set; }

        public string AlfaLogicExp { get; set; }

        public Dictionary<string, List<string>> ActionByAgents { get; set; }

        public string PiLogicExp { get; set; }

        public AfterQueryViewModel(AfterQueryNecOrPos afterQueryType, string alfaLogicexp,
            Dictionary<string, List<string>> actionsByAgents, string piLogixExp)
        {
            this.AfterQueryType = afterQueryType;
            this.AlfaLogicExp = alfaLogicexp;
            this.ActionByAgents = actionsByAgents;
            this.PiLogicExp = piLogixExp;
        }


        public static AfterQueryNecOrPos GetAfterQueryTypeFromView(StackPanel afterQueryStackPanel)
        {
            var typeComboBox = afterQueryStackPanel.Children[0] as ComboBox;
            if (typeComboBox == null)
                throw new ApplicationException("Null ref.");
            var selectedComboBoxItem = typeComboBox.SelectedItem as string;
            if (selectedComboBoxItem == null)
                throw new ApplicationException("Null ref.");
            return selectedComboBoxItem == "possibly" ? AfterQueryNecOrPos.Possibly : AfterQueryNecOrPos.Necessary;
        }

        public static string GetAlfaLogicExpFromView(StackPanel afterQueryStackPanel)
        {
            var alfaLogicExpTextBox = afterQueryStackPanel.Children[1] as TextBox;
            if (alfaLogicExpTextBox == null)
                return null;
            return alfaLogicExpTextBox.Text;
        }

        public static Dictionary<string, List<string>> GetActionsByAgentsFromView(StackPanel afterQueryStackPanel)
        {
            var actionsByAgents = new Dictionary<string, List<string>>();

            var expander = afterQueryStackPanel.Children[3] as Expander;
            if (expander == null)
                return null;
            var outerStackPanel = expander.Content as StackPanel;
            if (outerStackPanel == null)
                return null;
            for (int i = 1; i < outerStackPanel.Children.Count; i++)
            {
                try
                {
                    var innerStackPanel = outerStackPanel.Children[i] as StackPanel;
                    if (innerStackPanel == null)
                        return null;

                    var actionComboBox = innerStackPanel.Children[0] as ComboBox;
                    if (actionComboBox == null)
                        return null;
                    string action = actionComboBox.SelectedItem as String;
                    if (action == null)
                        action = String.Empty;
                    ;

                    var agentsExpander = innerStackPanel.Children[2] as Expander;
                    if (agentsExpander == null)
                        return null;
                    var agentsListBox = agentsExpander.Content as ListBox;
                    if (agentsListBox == null)
                        return null;


                    List<string> agents = new List<string>();
                    foreach (var agent in agentsListBox.SelectedItems)
                    {
                        agents.Add(agent as string);
                    }

                    if (!actionsByAgents.ContainsKey(action))
                    {
                        actionsByAgents.Add(action, agents);
                    }
                    else
                    {
                        throw new ApplicationException("Action is already declared.");
                    }
                }
                catch (ApplicationException e)
                {
                    MessageBox.Show(e.Message.ToString());
                }
            }

            return actionsByAgents;
        }

        public static string GetPiLogicExpFromView(StackPanel afterQueryStackPanel)
        {
            var piLogicExpTextBox = afterQueryStackPanel.Children[5] as TextBox;
            if (piLogicExpTextBox == null)
                return null;
            return piLogicExpTextBox.Text;
        }

        public enum AfterQueryNecOrPos
        {
            Possibly,
            Necessary
        }
    }
}