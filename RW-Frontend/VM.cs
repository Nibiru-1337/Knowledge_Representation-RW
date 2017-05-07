using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RW_Frontend.Annotations;

[assembly: InternalsVisibleTo("RW-tests")]

namespace RW_Frontend
{
    class VM : INotifyPropertyChanged
    {
        //TODO dopracować reprezentacje zdań - kolekcje string na vm zdań, wprowadzić właściwości z OnPropertyChanged()
        public ObservableCollection<string> Fluents { get; set; }
        public ObservableCollection<string> Actions { get; set; }
        public ObservableCollection<string> Agents { get; set; }

        public ObservableCollection<string> Noninertial { get; set; }

        public ObservableCollection<string> AlwaysStatements { get; set; }
        public ObservableCollection<string> InitiallyStatements { get; set; }

        public ObservableCollection<string> AfterStatements { get; set; }
        public ObservableCollection<string> CausesStatements { get; set; }
        public ObservableCollection<string> ReleasesStatements { get; set; }

        public VM()
        {
            Fluents = new ObservableCollection<string>();
            Actions = new ObservableCollection<string>();
            Agents = new ObservableCollection<string>();
            Noninertial = new ObservableCollection<string>();
            AlwaysStatements = new ObservableCollection<string>();
            InitiallyStatements = new ObservableCollection<string>();
            AfterStatements = new ObservableCollection<string>();
            CausesStatements = new ObservableCollection<string>();
            ReleasesStatements = new ObservableCollection<string>();
        }

        public static VM Create()
        {
            return new VM();
        }

        internal static VM Create(string[] fluents, string[] actions, string[] agents,
            string[] noninertial, string[] always, string[] initially,
            string[] after, string[] causes, string[] releases)
        {
            //metoda na potrzeby automatyzacji testów
            return new VM
            {
                //trzeba zaktualizować przy zmianie reprezentacji zdań 
                Fluents = new ObservableCollection<string>(fluents),
                Actions = new ObservableCollection<string>(actions),
                Agents = new ObservableCollection<string>(agents),
                Noninertial = new ObservableCollection<string>(noninertial),
                AlwaysStatements = new ObservableCollection<string>(always),
                InitiallyStatements = new ObservableCollection<string>(initially),
                AfterStatements = new ObservableCollection<string>(after),
                CausesStatements = new ObservableCollection<string>(causes),
                ReleasesStatements = new ObservableCollection<string>(releases)
            };
        }

        #region Components Control

        private TextBox CreateFluentAgentActionTextBox()
        {
            var textBox = new TextBox() {Height = 25, FontSize = 14, Margin = new Thickness(5)};
            textBox.BorderBrush = System.Windows.Media.Brushes.Red;
            textBox.LostFocus += ValidateTextBox;
            return textBox;
        }

        private void ValidateTextBox(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (string.IsNullOrEmpty(textBox.Text) ||
                (FluentsTextBoxes.Contains(textBox) &&
                 FluentsTextBoxes.Select(_ => _.Text).Count(_ => _.Equals(textBox.Text)) > 1)
                ||
                (AgentsTextBoxes.Contains(textBox) &&
                 AgentsTextBoxes.Select(_ => _.Text).Count(_ => _.Equals(textBox.Text)) > 1)
                ||
                (ActionsTextBoxes.Contains(textBox) &&
                 ActionsTextBoxes.Select(_ => _.Text).Count(_ => _.Equals(textBox.Text)) > 1))
            {
                textBox.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            else
            {
                textBox.BorderBrush = System.Windows.Media.Brushes.Black;
            }
        }

        private Button CreateRemoveButton(string inputDataType)
        {
            var button = new Button()
            {
                Height = 25,
                Width = 25,
                FontSize = 14,
                Content = "X",
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };

            switch (inputDataType)
            {
                case "Fluent":
                    button.Click += RemoveFluentButtonClick;
                    break;
                case "Action":
                    button.Click += RemoveActionButtonClick;
                    break;
                case "Agent":
                    button.Click += RemoveAgentButtonClick;
                    break;
                case "Causes":
                    button.Click += RemoveCausesClauseButtonClick;
                    break;
                case "AfterQuery":
                    button.Click += RemoveAfterQueryButtonClick;
                    break;
                case "ExecutableQuery":
                    button.Click += RemoveExecutableQueryButtonClick;
                    break;
                case "EngagedQuery":
                    button.Click += RemoveEngagedQueryButtonClick;
                    break;
            }
            return button;
        }

        private ComboBox CreateActionsComboBox()
        {
            var comboBox = new ComboBox() {Name = "ActionsComboBox", Margin = new Thickness(5), Height = 25, Width = 150, VerticalAlignment = VerticalAlignment.Center};
            comboBox.SelectedItem = String.Empty;
            comboBox.MouseEnter += (s, e) =>
            {
                var actions = ActionsTextBoxes.Select(_ => _.Text).Where(_ => _ != String.Empty);
                actions = actions.Concat(new List<string>() {String.Empty});
                comboBox.ItemsSource = actions;
            };
            return comboBox;
        }

        private Expander CreateAgentsExpanderListBox()
        {
            var ep = new Expander() {Name = "AgentsExpanderListBox", Header = "Agenci", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(10)};
            var listBox = new ListBox()
            {
                Name = "AgentsListBox",
                Margin = new Thickness(5),
                SelectionMode = SelectionMode.Multiple
            };
            ep.Content = listBox;
            ep.MouseEnter += (s, e) =>
            {
                var actions = AgentsTextBoxes.Select(_ => _.Text).Where(_ => _ != String.Empty);
                actions = actions.Concat(new List<string>() { String.Empty });
                listBox.ItemsSource = actions;
            };
            return ep;
        }

        private Label CreateByLabel()
        {
            var label = new Label() {Margin = new Thickness(5), Content = "by", VerticalAlignment = VerticalAlignment.Center};
            return label;
        }

        private Label CreateIfLabel()
        {
            var label = new Label() {Margin = new Thickness(5), Content = "if", VerticalAlignment = VerticalAlignment.Center};
            return label;
        }

        private Label CreateFromLabel()
        {
            var label = new Label() { Margin = new Thickness(5), Content = "from", VerticalAlignment = VerticalAlignment.Center };
            return label;
        }

        private TextBox CreateLogicExpTextBox(string name)
        {
            var textBox = new TextBox()
            {
                Height = 25,
                Width = 250,
                FontSize = 14,
                Margin = new Thickness(5),
                Name = name,
                VerticalAlignment = VerticalAlignment.Center
            };
            return textBox;
        }

        #region ActionByAgents control
        private StackPanel CreateActionByAgentsControlButtons(Expander expander)
        {
            var stackPanel = new StackPanel() {Orientation = Orientation.Horizontal};

            var addingButton = new Button() {Content = "+", Height = 20, Width = 20, Margin = new Thickness(5), FontSize = 12};
            addingButton.Click += (s, e) =>
            {
                var expanderContent = expander.Content as StackPanel;
                if(expanderContent == null)
                    return;
                expanderContent.Children.Add(CreateActionByAgentsStackPanel());
            };

            var removingButton = new Button() {Content = "x", Height = 20, Width = 20, Margin = new Thickness(5), FontSize = 12};
            removingButton.Click += (s, e) =>
            {
                var expanderContent = expander.Content as StackPanel;
                if (expanderContent == null)
                    return;
                if(expanderContent.Children.Count > 1)
                    expanderContent.Children.RemoveAt(expanderContent.Children.Count - 1);
            };

            stackPanel.Children.Add(addingButton);
            stackPanel.Children.Add(removingButton);
            return stackPanel;
        }

        private void AddActionByAgentsStackPanel(object sender, RoutedEventArgs e)
        {
            
        }

        private void RemoveActionByAgentsStackPanel(object sender, RoutedEventArgs e)
        {

        }

        private StackPanel CreateActionByAgentsStackPanel()
        {
            var stackPanel = new StackPanel() {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(CreateActionsComboBox());
            stackPanel.Children.Add(CreateByLabel());
            stackPanel.Children.Add(CreateAgentsExpanderListBox());
            return stackPanel;
        }

        private Expander CreateActionsByAgentsExpander()
        {
            var expander = new Expander() { Name = "ActionsByAgentsExpander", Header = "A by G", VerticalAlignment = VerticalAlignment.Center};
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(CreateActionByAgentsControlButtons(expander));
            stackPanel.Children.Add(CreateActionByAgentsStackPanel());
            expander.Content = stackPanel;
            return expander;
        }

        #endregion

        private bool CanDo()
        {
            return true;
        }

        #region Fluents

        public ICommand AddFluentCommand
        {
            get { return new RelayCommand(AddFluent, CanDo); }
        }

        private void AddFluent()
        {
            FluentsTextBoxes.Add(CreateFluentAgentActionTextBox());
            FluentsRemoveButtons.Add(CreateRemoveButton("Fluent"));
        }

        private void RemoveFluentButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            int removingItemIdx = FluentsRemoveButtons.IndexOf(clickedButton);
            FluentsRemoveButtons.RemoveAt(removingItemIdx);
            FluentsTextBoxes.RemoveAt(removingItemIdx);
        }

        #region Fluents -> TextBoxes

        private ObservableCollection<TextBox> _fluentsTextBoxes;

        public ObservableCollection<TextBox> FluentsTextBoxes
        {
            get
            {
                if (_fluentsTextBoxes == null)
                {
                    _fluentsTextBoxes = new ObservableCollection<TextBox>()
                    {
                        CreateFluentAgentActionTextBox()
                    };
                }
                return _fluentsTextBoxes;
            }
        }

        #endregion

        #region Fluents -> RemoveButtons

        private ObservableCollection<Button> _fluentsRemoveButtons;

        public ObservableCollection<Button> FluentsRemoveButtons
        {
            get
            {
                if (_fluentsRemoveButtons == null)
                {
                    _fluentsRemoveButtons = new ObservableCollection<Button>()
                    {
                        CreateRemoveButton("Fluent")
                    };
                }
                return _fluentsRemoveButtons;
            }
        }

        #endregion

        #endregion

        #region Actions

        public ICommand AddActionCommand
        {
            get { return new RelayCommand(AddAction, CanDo); }
        }

        private void AddAction()
        {
            ActionsTextBoxes.Add(CreateFluentAgentActionTextBox());
            ActionsRemoveButtons.Add(CreateRemoveButton("Action"));
        }

        private void RemoveActionButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            int removingItemIdx = ActionsRemoveButtons.IndexOf(clickedButton);
            ActionsRemoveButtons.RemoveAt(removingItemIdx);
            string removedValue = ActionsTextBoxes.ElementAt(removingItemIdx).Text;
            ActionsTextBoxes.RemoveAt(removingItemIdx);
            //ValidateActionComboBoxes(removedValue);
        }

        //private void ValidateActionComboBoxes(string removedValue)
        //{
        //    foreach (var stackPanel in CausesClausesStackPanels)
        //    {
        //        foreach (var child in stackPanel.Children)
        //        {
        //            if (child is ComboBox)
        //            {
        //                var cb = child as ComboBox;
        //                if (cb.Name == "ActionsComboBox" && (string) (cb.SelectedItem) == removedValue)
        //                    cb.SelectedItem = String.Empty;
        //            }
        //        }
        //    }
        //}

        #region Actions -> TextBoxes

        private ObservableCollection<TextBox> _actionsTextBoxes;

        public ObservableCollection<TextBox> ActionsTextBoxes
        {
            get
            {
                if (_actionsTextBoxes == null)
                {
                    _actionsTextBoxes = new ObservableCollection<TextBox>()
                    {
                        CreateFluentAgentActionTextBox()
                    };
                }
                return _actionsTextBoxes;
            }
        }

        #endregion

        #region Actions -> RemoveButtons

        private ObservableCollection<Button> _actionsRemoveButtons;

        public ObservableCollection<Button> ActionsRemoveButtons
        {
            get
            {
                if (_actionsRemoveButtons == null)
                {
                    _actionsRemoveButtons = new ObservableCollection<Button>()
                    {
                        CreateRemoveButton("Action")
                    };
                }
                return _actionsRemoveButtons;
            }
        }

        #endregion

        #endregion

        #region Agents

        public ICommand AddAgentCommand
        {
            get { return new RelayCommand(AddAgent, CanDo); }
        }

        private void AddAgent()
        {
            AgentsTextBoxes.Add(CreateFluentAgentActionTextBox());
            AgentsRemoveButtons.Add(CreateRemoveButton("Agent"));
        }

        private void RemoveAgentButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            int removingItemIdx = AgentsRemoveButtons.IndexOf(clickedButton);
            AgentsRemoveButtons.RemoveAt(removingItemIdx);
            AgentsTextBoxes.RemoveAt(removingItemIdx);
        }

        #region Actions -> TextBoxes

        private ObservableCollection<TextBox> _agentsTextBoxes;

        public ObservableCollection<TextBox> AgentsTextBoxes
        {
            get
            {
                if (_agentsTextBoxes == null)
                {
                    _agentsTextBoxes = new ObservableCollection<TextBox>()
                    {
                        CreateFluentAgentActionTextBox()
                    };
                }
                return _agentsTextBoxes;
            }
        }

        #endregion

        #region Actions -> RemoveButtons

        private ObservableCollection<Button> _agentsRemoveButtons;

        public ObservableCollection<Button> AgentsRemoveButtons
        {
            get
            {
                if (_agentsRemoveButtons == null)
                {
                    _agentsRemoveButtons = new ObservableCollection<Button>()
                    {
                        CreateRemoveButton("Agent")
                    };
                }
                return _agentsRemoveButtons;
            }
        }

        #endregion

        #endregion

        #region Causes clauses

        public ICommand AddCausesClauseCommand
        {
            get { return new RelayCommand(AddCausesClause, CanDo); }
        }

        private void AddCausesClause()
        {
            CausesClausesStackPanels.Add(CreateCausesClauseStackPanel());
        }

        private StackPanel CreateCausesClauseStackPanel()
        {
            var stackPanel = new StackPanel() {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(CreateActionsComboBox());
            stackPanel.Children.Add(CreateByLabel());
            stackPanel.Children.Add(CreateAgentsExpanderListBox());
            stackPanel.Children.Add(new Label() {Margin = new Thickness(5), Content = "causes", VerticalAlignment = VerticalAlignment.Center});
            stackPanel.Children.Add(CreateLogicExpTextBox("alfaCausesExp"));
            stackPanel.Children.Add(CreateIfLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piCausesExp"));
            stackPanel.Children.Add(CreateRemoveButton("Causes"));
            return stackPanel;
        }

        private void RemoveCausesClauseButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            var removingStackPanel = CausesClausesStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = CausesClausesStackPanels.IndexOf(removingStackPanel);
            CausesClausesStackPanels.RemoveAt(removingItemIdx);
        }

        #region CausesClauses -> StackPanels

        private ObservableCollection<StackPanel> _causesClausesStackPanels;

        public ObservableCollection<StackPanel> CausesClausesStackPanels
        {
            get
            {
                if (_causesClausesStackPanels == null)
                {
                    _causesClausesStackPanels = new ObservableCollection<StackPanel>()
                    {
                        CreateCausesClauseStackPanel()
                    };
                }
                return _causesClausesStackPanels;
            }
        }

        #endregion

        #endregion

        #region ExecutableQuery
        public ICommand AddExecutableQueryCommand
        {
            get { return new RelayCommand(AddExecutableQuery, CanDo); }
        }

        private void AddExecutableQuery()
        {
            ExecutableQueryStackPanels.Add(CreateExecutableQueryStackPanel());
        }

        private StackPanel CreateExecutableQueryStackPanel()
        {
            var stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new ComboBox() { Margin = new Thickness(10), Height = 25, Width = 150, VerticalAlignment = VerticalAlignment.Center, ItemsSource = new List<string>() { "always", String.Empty }, SelectedItem = "always" });
            stackPanel.Children.Add(new Label() { Margin = new Thickness(5), Content = "executable", VerticalAlignment = VerticalAlignment.Center });
            stackPanel.Children.Add(CreateActionsByAgentsExpander());
            stackPanel.Children.Add(CreateFromLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piExecutableQueryExp"));
            stackPanel.Children.Add(CreateRemoveButton("ExecutableQuery"));

            return stackPanel;
        }

        private void RemoveExecutableQueryButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            var removingStackPanel = AfterQueryStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = AfterQueryStackPanels.IndexOf(removingStackPanel);
            AfterQueryStackPanels.RemoveAt(removingItemIdx);
        }

        #region ExecutableQuery -> StackPanels

        private ObservableCollection<StackPanel> _executableQueryStackPanels;

        public ObservableCollection<StackPanel> ExecutableQueryStackPanels
        {
            get
            {
                if (_executableQueryStackPanels == null)
                {
                    _executableQueryStackPanels = new ObservableCollection<StackPanel>()
                    {
                        CreateExecutableQueryStackPanel()
                    };
                }
                return _executableQueryStackPanels;
            }
        }
        #endregion

        #endregion

        #region AfterQuery
        public ICommand AddAfterQueryCommand
        {
            get { return new RelayCommand(AddAfterQuery, CanDo); }
        }

        private void AddAfterQuery()
        {
            AfterQueryStackPanels.Add(CreateAfterQueryStackPanel());
        }

        private StackPanel CreateAfterQueryStackPanel()
        {
            var stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(new ComboBox() { Margin = new Thickness(10), Height = 25, Width = 150, VerticalAlignment = VerticalAlignment.Center, ItemsSource = new List<string>() {"possibly", "necessary"}, SelectedItem = "possibly"});
            stackPanel.Children.Add(CreateLogicExpTextBox("alfaAfterQueryExp"));
            stackPanel.Children.Add(new Label() { Margin = new Thickness(5), Content = "after", VerticalAlignment = VerticalAlignment.Center });
            stackPanel.Children.Add(CreateActionsByAgentsExpander());
            stackPanel.Children.Add(CreateFromLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piAfterQueryExp"));
            stackPanel.Children.Add(CreateRemoveButton("AfterQuery"));
            return stackPanel;
        }

        private void RemoveAfterQueryButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            var removingStackPanel = AfterQueryStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = AfterQueryStackPanels.IndexOf(removingStackPanel);
            AfterQueryStackPanels.RemoveAt(removingItemIdx);
        }

        #region AfterQuery -> StackPanels

        private ObservableCollection<StackPanel> _afterQueryStackPanels;

        public ObservableCollection<StackPanel> AfterQueryStackPanels
        {
            get
            {
                if (_afterQueryStackPanels == null)
                {
                    _afterQueryStackPanels = new ObservableCollection<StackPanel>()
                    {
                        CreateAfterQueryStackPanel()
                    };
                }
                return _afterQueryStackPanels;
            }
        }
        #endregion

        #endregion

        #region EngagedQuery
        public ICommand AddEngagedQueryCommand
        {
            get { return new RelayCommand(AddEngagedQuery, CanDo); }
        }

        private void AddEngagedQuery()
        {
            EngagedQueryStackPanels.Add(CreateEngagedQueryStackPanel());
        }

        private StackPanel CreateEngagedQueryStackPanel()
        {
            var stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(CreateAgentsExpanderListBox());
            stackPanel.Children.Add(new ComboBox()
            {
                Margin = new Thickness(10),
                Height = 25,
                Width = 150,
                VerticalAlignment = VerticalAlignment.Center,
                ItemsSource = new List<string>() {"always", String.Empty},
                SelectedItem = "always"
            });
            stackPanel.Children.Add(new Label() { Margin = new Thickness(5), Content = "engaged in", VerticalAlignment = VerticalAlignment.Center });
            stackPanel.Children.Add(CreateActionsByAgentsExpander());
            stackPanel.Children.Add(CreateFromLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piEngagedQueryExp"));
            stackPanel.Children.Add(CreateRemoveButton("EngagedQuery"));
            return stackPanel;
        }

        private void RemoveEngagedQueryButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            var removingStackPanel = EngagedQueryStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = EngagedQueryStackPanels.IndexOf(removingStackPanel);
            EngagedQueryStackPanels.RemoveAt(removingItemIdx);
        }

        #region AfterQuery -> StackPanels

        private ObservableCollection<StackPanel> _engagedQueryStackPanels;

        public ObservableCollection<StackPanel> EngagedQueryStackPanels
        {
            get
            {
                if (_engagedQueryStackPanels == null)
                {
                    _engagedQueryStackPanels = new ObservableCollection<StackPanel>()
                    {
                        CreateEngagedQueryStackPanel()
                    };
                }
                return _engagedQueryStackPanels;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Necessary VM snippet

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}