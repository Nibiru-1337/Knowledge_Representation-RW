using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RW_Frontend.InputsViewModels
{
    public class ObservableClauseViewModel
    {
        public string AlfaLogicExp { get; set; }

        public List<Tuple<string, List<string>>> ActionByAgents { get; set; }

        public ObservableClauseViewModel(string alfaLogicexp, List<Tuple<string, List<string>>> actionsByAgents)
        {
            AlfaLogicExp = alfaLogicexp;
            ActionByAgents = actionsByAgents;
        }

        public static string GetAlfaLogicExpFromView(StackPanel observableClauseStackPanel)
        {
            var alfaLogicExpTextBox = observableClauseStackPanel.Children[1] as TextBox;
            if (alfaLogicExpTextBox == null)
                return null;
            return alfaLogicExpTextBox.Text;
        }

        public static List<Tuple<string, List<string>>> GetActionsByAgentsFromView(StackPanel observableClauseStackPanel)
        {
            var actionsByAgents = new List<Tuple<string, List<string>>>();

            var expander = observableClauseStackPanel.Children[3] as Expander;
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

                    actionsByAgents.Add(new Tuple<string, List<string>>(action, agents));
                }
                catch (ApplicationException e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            return actionsByAgents;
        }
    }
}