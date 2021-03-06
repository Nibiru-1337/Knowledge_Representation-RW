﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RW_backend.Models.World;
using RW_Frontend.Annotations;
using RW_Frontend.InputsViewModels;
using RW_Frontend.Properties;

[assembly: InternalsVisibleTo("RW-tests")]

namespace RW_Frontend
{
    public class VM : INotifyPropertyChanged
    {
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

        public World World { get; private set; }

        #region Components Control

        private TextBox CreateFluentAgentActionTextBox()
        {
            var textBox = new TextBox {Height = 25, FontSize = 14, Margin = new Thickness(5)};
            textBox.BorderBrush = Brushes.Red;
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
                textBox.BorderBrush = Brushes.Red;
            }
            else
            {
                textBox.BorderBrush = Brushes.Black;
            }
        }

        private Button CreateRemoveButton(string inputDataType)
        {
            var button = new Button
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
                    button.ToolTip = Settings.Default.RemoveFluent;
                    break;
                case "Action":
                    button.Click += RemoveActionButtonClick;
                    button.ToolTip = Settings.Default.RemoveAction;
                    break;
                case "Agent":
                    button.Click += RemoveAgentButtonClick;
                    button.ToolTip = Settings.Default.RemoveAgent;
                    break;
                case "Initially":
                    button.Click += RemoveInitiallyClauseButtonClick;
                    button.ToolTip = Settings.Default.RemoveInitially;
                    break;
                case "After":
                    button.Click += RemoveAfterClauseButtonClick;
                    button.ToolTip = Settings.Default.RemoveAfter;
                    break;
                case "Observable":
                    button.Click += RemoveObservableClauseButtonClick;
                    button.ToolTip = Settings.Default.RemoveObservable;
                    break;
                case "Causes":
                    button.Click += RemoveCausesClauseButtonClick;
                    button.ToolTip = Settings.Default.RemoveCauses;
                    break;
                case "Impossible":
                    button.Click += RemoveImpossibleClauseButtonClick;
                    button.ToolTip = Settings.Default.RemoveImpossible;
                    break;
                case "Releases":
                    button.Click += RemoveReleasesClauseButtonClick;
                    button.ToolTip = Settings.Default.RemoveReleases;
                    break;
                case "Always":
                    button.Click += RemoveAlwaysClauseButtonClick;
                    button.ToolTip = Settings.Default.RemoveAlways;
                    break;
                case "Noninertial":
                    button.Click += RemoveNoninertialClauseButtonClick;
                    button.ToolTip = Settings.Default.RemoveNoninertial;
                    break;
                case "AfterQuery":
                    button.Click += RemoveAfterQueryButtonClick;
                    button.ToolTip = Settings.Default.RemoveAfterQuery;
                    break;
                case "ExecutableQuery":
                    button.Click += RemoveExecutableQueryButtonClick;
                    button.ToolTip = Settings.Default.RemoveExecutableQuery;
                    break;
                case "EngagedQuery":
                    button.Click += RemoveEngagedQueryButtonClick;
                    button.ToolTip = Settings.Default.RemoveExecutableQuery;
                    break;
            }
            return button;
        }

        private Button CreateCalculateQueryButton(string queryType)
        {
            var button = new Button
            {
                Height = 25,
                FontSize = 14,
                Content = "Oblicz",
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };
            switch (queryType)
            {
                case "Executable":
                    button.Click += CalculateExecutableQuery;
                    button.ToolTip = Settings.Default.CalculateExecutableQuery;
                    break;
                case "After":
                    button.Click += CalculateAfterQuery;
                    button.ToolTip = Settings.Default.CalculateAfterQuery;
                    break;
                case "Engaged":
                    button.Click += CalculateEngagedQuery;
                    button.ToolTip = Settings.Default.CalculateEngagedQuery;
                    break;
            }
            return button;
        }

        private ComboBox CreateActionsComboBox()
        {
            var comboBox = new ComboBox
            {
                Name = "ActionsComboBox",
                Margin = new Thickness(5),
                Height = 25,
                Width = 150,
                VerticalAlignment = VerticalAlignment.Center
            };
            comboBox.ItemsSource = new List<string>() {string.Empty};
            comboBox.SelectedItem = string.Empty;
            comboBox.MouseEnter += (s, e) =>
            {
                var actions = ActionsTextBoxes.Select(_ => _.Text).Where(_ => _ != string.Empty);
                actions = actions.Concat(new List<string> {string.Empty});
                comboBox.ItemsSource = actions;
            };
            return comboBox;
        }

        private ComboBox CreateFluentsComboBox()
        {
            var comboBox = new ComboBox
            {
                Name = "FluentsComboBox",
                Margin = new Thickness(5),
                Height = 25,
                Width = 150,
                VerticalAlignment = VerticalAlignment.Center
            };
            comboBox.ItemsSource = new List<string>() {string.Empty};
            comboBox.SelectedItem = string.Empty;
            comboBox.MouseEnter += (s, e) =>
            {
                var fluents = FluentsTextBoxes.Select(_ => _.Text).Where(_ => _ != string.Empty);
                fluents = fluents.Concat(new List<string> {string.Empty});
                comboBox.ItemsSource = fluents;
            };
            return comboBox;
        }

        internal const string AnyAgent = "ANY";

        private Expander CreateAgentsExpanderListBox()
        {
            var ep = new Expander
            {
                Name = "AgentsExpanderListBox",
                Header = "Agents",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10)
            };
            var listBox = new ListBox
            {
                Name = "AgentsListBox",
                Margin = new Thickness(5),
                SelectionMode = SelectionMode.Multiple,
                SelectedItem = string.Empty
            };
            listBox.SelectionChanged += new SelectionChangedEventHandler(OnAgentsListBoxChanged);
            ep.Content = listBox;
            ep.MouseEnter += (s, e) =>
            {
                var actions = AgentsTextBoxes.Select(_ => _.Text).Where(_ => _ != string.Empty);
                actions = actions.Concat(new List<string> {AnyAgent});
                listBox.ItemsSource = actions;
            };
            
            return ep;
        }

        private void OnAgentsListBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null)
                return;
            if (listBox.SelectedItems.Contains("ANY"))
            {
                listBox.SelectionMode= SelectionMode.Single;
                listBox.SelectedIndex = listBox.Items.IndexOf("ANY");
            }
            else if (!listBox.SelectedItems.Contains("ANY"))
            {
                listBox.SelectionMode = SelectionMode.Multiple;
            }
        }

        private Label CreateByLabel()
        {
            var label = new Label
            {
                Margin = new Thickness(5),
                Content = "by",
                VerticalAlignment = VerticalAlignment.Center
            };
            return label;
        }

        private Label CreateWithLabel()
        {
            var label = new Label
            {
                Margin = new Thickness(5),
                Content = "with",
                VerticalAlignment = VerticalAlignment.Center
            };
            return label;
        }

        private Label CreateIfLabel()
        {
            var label = new Label
            {
                Margin = new Thickness(5),
                Content = "if",
                VerticalAlignment = VerticalAlignment.Center
            };
            return label;
        }

        private Label CreateFromLabel()
        {
            var label = new Label
            {
                Margin = new Thickness(5),
                Content = "from",
                VerticalAlignment = VerticalAlignment.Center
            };
            return label;
        }

        private Label CreateQueryResultLabel()
        {
            var label = new Label
            {
                Margin = new Thickness(10),
                Content = "T/N",
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold,
                BorderBrush = Brushes.Gray,
                Foreground = Brushes.Gray,
                BorderThickness = new Thickness(2)
            };
            return label;
        }

        private TextBox CreateLogicExpTextBox(string name)
        {
            var textBox = new TextBox
            {
                Height = 25,
                Width = 150,
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
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};

            var addingButton = new Button
            {
                Content = "+",
                Height = 20,
                Width = 20,
                Margin = new Thickness(5),
                FontSize = 12,
                ToolTip = Settings.Default.AddScenario
            };
            addingButton.Click += (s, e) =>
            {
                var expanderContent = expander.Content as StackPanel;
                if (expanderContent == null)
                    return;
                expanderContent.Children.Add(CreateActionByAgentsStackPanel());
            };

            var removingButton = new Button
            {
                Content = "x",
                Height = 20,
                Width = 20,
                Margin = new Thickness(5),
                FontSize = 12,
                ToolTip = Settings.Default.RemoveScenario
            };
            removingButton.Click += (s, e) =>
            {
                var expanderContent = expander.Content as StackPanel;
                if (expanderContent == null)
                    return;
                if (expanderContent.Children.Count > 1)
                    expanderContent.Children.RemoveAt(expanderContent.Children.Count - 1);
            };

            stackPanel.Children.Add(addingButton);
            stackPanel.Children.Add(removingButton);
            return stackPanel;
        }

        private StackPanel CreateActionByAgentsStackPanel()
        {
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(CreateActionsComboBox());
            stackPanel.Children.Add(CreateByLabel());
            stackPanel.Children.Add(CreateAgentsExpanderListBox());
            return stackPanel;
        }

        private Expander CreateActionsByAgentsExpander()
        {
            var expander = new Expander
            {
                Name = "ActionsByAgentsExpander",
                Header = "A by G",
                VerticalAlignment = VerticalAlignment.Center
            };
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
            if (FluentsTextBoxes.Count >= 13)
                return;
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
                    _fluentsTextBoxes = new ObservableCollection<TextBox>
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
                    _fluentsRemoveButtons = new ObservableCollection<Button>
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
                    _actionsTextBoxes = new ObservableCollection<TextBox>
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
                    _actionsRemoveButtons = new ObservableCollection<Button>
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
                    _agentsTextBoxes = new ObservableCollection<TextBox>
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
                    _agentsRemoveButtons = new ObservableCollection<Button>
                    {
                        CreateRemoveButton("Agent")
                    };
                }
                return _agentsRemoveButtons;
            }
        }

        #endregion

        #endregion

        #region Initially clauses

        public ICommand AddInitiallyClauseCommand
        {
            get { return new RelayCommand(AddInitiallyClause, CanDo); }
        }

        private void AddInitiallyClause()
        {
            InitiallyClausesStackPanels.Add(CreateInitiallyClauseStackPanel());
        }

        private StackPanel CreateInitiallyClauseStackPanel()
        {
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "initially",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateLogicExpTextBox("alfaInitiallyExp"));
            stackPanel.Children.Add(CreateRemoveButton("Initially"));
            return stackPanel;
        }

        private void RemoveInitiallyClauseButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            var removingStackPanel = InitiallyClausesStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = InitiallyClausesStackPanels.IndexOf(removingStackPanel);
            InitiallyClausesStackPanels.RemoveAt(removingItemIdx);
        }

        #region InitiallyClauses -> StackPanels

        private ObservableCollection<StackPanel> _initiallyClausesStackPanels;

        public ObservableCollection<StackPanel> InitiallyClausesStackPanels
        {
            get
            {
                if (_initiallyClausesStackPanels == null)
                {
                    _initiallyClausesStackPanels = new ObservableCollection<StackPanel>
                    {
                        CreateInitiallyClauseStackPanel()
                    };
                }
                return _initiallyClausesStackPanels;
            }
        }

        #endregion

        #endregion

        #region After clauses

        public ICommand AddAfterClauseCommand
        {
            get { return new RelayCommand(AddAfterClause, CanDo); }
        }

        private void AddAfterClause()
        {
            AfterClausesStackPanels.Add(CreateAfterClauseStackPanel());
        }

        private StackPanel CreateAfterClauseStackPanel()
        {
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(CreateLogicExpTextBox("alfaAfterExp"));
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "after",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateActionsByAgentsExpander());
            stackPanel.Children.Add(CreateRemoveButton("After"));
            return stackPanel;
        }

        private void RemoveAfterClauseButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            var removingStackPanel = AfterClausesStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = AfterClausesStackPanels.IndexOf(removingStackPanel);
            AfterClausesStackPanels.RemoveAt(removingItemIdx);
        }

        #region AfterClauses -> StackPanels

        private ObservableCollection<StackPanel> _afterClausesStackPanels;

        public ObservableCollection<StackPanel> AfterClausesStackPanels
        {
            get
            {
                if (_afterClausesStackPanels == null)
                {
                    _afterClausesStackPanels = new ObservableCollection<StackPanel>
                    {
                        CreateAfterClauseStackPanel()
                    };
                }
                return _afterClausesStackPanels;
            }
        }

        #endregion

        #endregion

        #region Observable clauses

        public ICommand AddObservableClauseCommand
        {
            get { return new RelayCommand(AddObservableClause, CanDo); }
        }

        private void AddObservableClause()
        {
            ObservableClausesStackPanels.Add(CreateObservableClauseStackPanel());
        }

        private StackPanel CreateObservableClauseStackPanel()
        {
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "observable",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateLogicExpTextBox("alfaObservableExp"));
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "after",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateActionsByAgentsExpander());
            stackPanel.Children.Add(CreateRemoveButton("Observable"));
            return stackPanel;
        }

        private void RemoveObservableClauseButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            var removingStackPanel = ObservableClausesStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = ObservableClausesStackPanels.IndexOf(removingStackPanel);
            ObservableClausesStackPanels.RemoveAt(removingItemIdx);
        }

        #region ObservableClauses -> StackPanels

        private ObservableCollection<StackPanel> _observableClausesStackPanels;

        public ObservableCollection<StackPanel> ObservableClausesStackPanels
        {
            get
            {
                if (_observableClausesStackPanels == null)
                {
                    _observableClausesStackPanels = new ObservableCollection<StackPanel>
                    {
                        CreateObservableClauseStackPanel()
                    };
                }
                return _observableClausesStackPanels;
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
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(CreateActionsComboBox());
            stackPanel.Children.Add(CreateByLabel());
            stackPanel.Children.Add(CreateAgentsExpanderListBox());
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "causes",
                VerticalAlignment = VerticalAlignment.Center
            });
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
                    _causesClausesStackPanels = new ObservableCollection<StackPanel>
                    {
                        CreateCausesClauseStackPanel()
                    };
                }
                return _causesClausesStackPanels;
            }
        }

        #endregion

        #endregion

        #region Impossible clauses

        public ICommand AddImpossibleClauseCommand
        {
            get { return new RelayCommand(AddImpossibleClause, CanDo); }
        }

        private void AddImpossibleClause()
        {
            ImpossibleClausesStackPanels.Add(CreateImpossibleClauseStackPanel());
        }

        private StackPanel CreateImpossibleClauseStackPanel()
        {
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "impossible",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateActionsComboBox());
            stackPanel.Children.Add(CreateWithLabel());
            stackPanel.Children.Add(CreateAgentsExpanderListBox());
            stackPanel.Children.Add(CreateIfLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piImpossibleExp"));
            stackPanel.Children.Add(CreateRemoveButton("Impossible"));
            return stackPanel;
        }

        private void RemoveImpossibleClauseButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            var removingStackPanel = ImpossibleClausesStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = ImpossibleClausesStackPanels.IndexOf(removingStackPanel);
            ImpossibleClausesStackPanels.RemoveAt(removingItemIdx);
        }

        #region ImpossibleClauses -> StackPanels

        private ObservableCollection<StackPanel> _impossibleClausesStackPanels;

        public ObservableCollection<StackPanel> ImpossibleClausesStackPanels
        {
            get
            {
                if (_impossibleClausesStackPanels == null)
                {
                    _impossibleClausesStackPanels = new ObservableCollection<StackPanel>
                    {
                        CreateImpossibleClauseStackPanel()
                    };
                }
                return _impossibleClausesStackPanels;
            }
        }

        #endregion

        #endregion

        #region Releases clauses

        public ICommand AddReleasesClauseCommand
        {
            get { return new RelayCommand(AddReleasesClause, CanDo); }
        }

        private void AddReleasesClause()
        {
            ReleasesClausesStackPanels.Add(CreateReleasesClauseStackPanel());
        }

        private StackPanel CreateReleasesClauseStackPanel()
        {
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(CreateActionsComboBox());
            stackPanel.Children.Add(CreateByLabel());
            stackPanel.Children.Add(CreateAgentsExpanderListBox());
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "releases",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateFluentsComboBox());
            stackPanel.Children.Add(CreateIfLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piReleasesExp"));
            stackPanel.Children.Add(CreateRemoveButton("Releases"));
            return stackPanel;
        }

        private void RemoveReleasesClauseButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            var removingStackPanel = ReleasesClausesStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = ReleasesClausesStackPanels.IndexOf(removingStackPanel);
            ReleasesClausesStackPanels.RemoveAt(removingItemIdx);
        }

        #region ReleasesClauses -> StackPanels

        private ObservableCollection<StackPanel> _releasesClausesStackPanels;

        public ObservableCollection<StackPanel> ReleasesClausesStackPanels
        {
            get
            {
                if (_releasesClausesStackPanels == null)
                {
                    _releasesClausesStackPanels = new ObservableCollection<StackPanel>
                    {
                        CreateReleasesClauseStackPanel()
                    };
                }
                return _releasesClausesStackPanels;
            }
        }

        #endregion

        #endregion

        #region Always clauses

        public ICommand AddAlwaysClauseCommand
        {
            get { return new RelayCommand(AddAlwaysClause, CanDo); }
        }

        private void AddAlwaysClause()
        {
            AlwaysClausesStackPanels.Add(CreateAlwaysClauseStackPanel());
        }

        private StackPanel CreateAlwaysClauseStackPanel()
        {
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "always",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateLogicExpTextBox("alfaAlwaysExp"));
            stackPanel.Children.Add(CreateRemoveButton("Always"));
            return stackPanel;
        }

        private void RemoveAlwaysClauseButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            var removingStackPanel = AlwaysClausesStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = AlwaysClausesStackPanels.IndexOf(removingStackPanel);
            AlwaysClausesStackPanels.RemoveAt(removingItemIdx);
        }

        #region AlwaysClauses -> StackPanels

        private ObservableCollection<StackPanel> _alwaysClausesStackPanels;

        public ObservableCollection<StackPanel> AlwaysClausesStackPanels
        {
            get
            {
                if (_alwaysClausesStackPanels == null)
                {
                    _alwaysClausesStackPanels = new ObservableCollection<StackPanel>
                    {
                        CreateAlwaysClauseStackPanel()
                    };
                }
                return _alwaysClausesStackPanels;
            }
        }

        #endregion

        #endregion

        #region Noninertial clauses

        public ICommand AddNoninertialClauseCommand
        {
            get { return new RelayCommand(AddNoninertialClause, CanDo); }
        }

        private void AddNoninertialClause()
        {
            NoninertialClausesStackPanels.Add(CreateNoninertialClauseStackPanel());
        }

        private StackPanel CreateNoninertialClauseStackPanel()
        {
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "noninertial",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateFluentsComboBox());
            stackPanel.Children.Add(CreateRemoveButton("Noninertial"));
            return stackPanel;
        }

        private void RemoveNoninertialClauseButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            var removingStackPanel = NoninertialClausesStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = NoninertialClausesStackPanels.IndexOf(removingStackPanel);
            NoninertialClausesStackPanels.RemoveAt(removingItemIdx);
        }

        #region NoninertialClauses -> StackPanels

        private ObservableCollection<StackPanel> _noninertialClausesStackPanels;

        public ObservableCollection<StackPanel> NoninertialClausesStackPanels
        {
            get
            {
                if (_noninertialClausesStackPanels == null)
                {
                    _noninertialClausesStackPanels = new ObservableCollection<StackPanel>
                    {
                        CreateNoninertialClauseStackPanel()
                    };
                }
                return _noninertialClausesStackPanels;
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
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(new ComboBox
            {
                Margin = new Thickness(10),
                Height = 25,
                Width = 150,
                VerticalAlignment = VerticalAlignment.Center,
                ItemsSource = new List<string> {"always", string.Empty},
                SelectedItem = "always"
            });
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "executable",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateActionsByAgentsExpander());
            stackPanel.Children.Add(CreateFromLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piExecutableQueryExp"));
            stackPanel.Children.Add(CreateRemoveButton("ExecutableQuery"));
            stackPanel.Children.Add(CreateCalculateQueryButton("Executable"));
            stackPanel.Children.Add(CreateQueryResultLabel());

            return stackPanel;
        }

        private void RemoveExecutableQueryButtonClick(object sender, RoutedEventArgs e)
        {
            var removingItemIdx = FindExecutableQueryIndexByButton((Button) sender);
            ExecutableQueryStackPanels.RemoveAt(removingItemIdx);
        }

        private int FindExecutableQueryIndexByButton(Button clickedButton)
        {
            var removingStackPanel = ExecutableQueryStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = ExecutableQueryStackPanels.IndexOf(removingStackPanel);
            return removingItemIdx;
        }

        #region ExecutableQuery -> StackPanels

        private ObservableCollection<StackPanel> _executableQueryStackPanels;

        public ObservableCollection<StackPanel> ExecutableQueryStackPanels
        {
            get
            {
                if (_executableQueryStackPanels == null)
                {
                    _executableQueryStackPanels = new ObservableCollection<StackPanel>
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
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(new ComboBox
            {
                Margin = new Thickness(10),
                Height = 25,
                Width = 150,
                VerticalAlignment = VerticalAlignment.Center,
                ItemsSource = new List<string> {"possibly", "necessary"},
                SelectedItem = "possibly"
            });
            stackPanel.Children.Add(CreateLogicExpTextBox("alfaAfterQueryExp"));
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "after",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateActionsByAgentsExpander());
            stackPanel.Children.Add(CreateFromLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piAfterQueryExp"));
            stackPanel.Children.Add(CreateRemoveButton("AfterQuery"));
            stackPanel.Children.Add(CreateCalculateQueryButton("After"));
            stackPanel.Children.Add(CreateQueryResultLabel());
            return stackPanel;
        }

        private void RemoveAfterQueryButtonClick(object sender, RoutedEventArgs e)
        {
            var removingItemIdx = FindAfterQueryIndexByButton((Button) sender);
            AfterQueryStackPanels.RemoveAt(removingItemIdx);
        }

        private int FindAfterQueryIndexByButton(Button clickedButton)
        {
            var removingStackPanel = AfterQueryStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = AfterQueryStackPanels.IndexOf(removingStackPanel);
            return removingItemIdx;
        }

        #region AfterQuery -> StackPanels

        private ObservableCollection<StackPanel> _afterQueryStackPanels;

        public ObservableCollection<StackPanel> AfterQueryStackPanels
        {
            get
            {
                if (_afterQueryStackPanels == null)
                {
                    _afterQueryStackPanels = new ObservableCollection<StackPanel>
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
            var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(CreateAgentsExpanderListBox());
            stackPanel.Children.Add(new ComboBox
            {
                Margin = new Thickness(10),
                Height = 25,
                Width = 150,
                VerticalAlignment = VerticalAlignment.Center,
                ItemsSource = new List<string> {"always", string.Empty},
                SelectedItem = "always"
            });
            stackPanel.Children.Add(new Label
            {
                Margin = new Thickness(5),
                Content = "engaged in",
                VerticalAlignment = VerticalAlignment.Center
            });
            stackPanel.Children.Add(CreateActionsByAgentsExpander());
            stackPanel.Children.Add(CreateFromLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piEngagedQueryExp"));
            stackPanel.Children.Add(CreateRemoveButton("EngagedQuery"));
            stackPanel.Children.Add(CreateCalculateQueryButton("Engaged"));
            stackPanel.Children.Add(CreateQueryResultLabel());

            return stackPanel;
        }

        private void RemoveEngagedQueryButtonClick(object sender, RoutedEventArgs e)
        {
            var removingItemIdx = FindEngagedQueryIndexByButton((Button) sender);
            EngagedQueryStackPanels.RemoveAt(removingItemIdx);
        }

        private int FindEngagedQueryIndexByButton(Button clickedButton)
        {
            var removingStackPanel = EngagedQueryStackPanels.First(_ => _.Children.Contains(clickedButton));
            int removingItemIdx = EngagedQueryStackPanels.IndexOf(removingStackPanel);
            return removingItemIdx;
        }

        #region EngagedQuery -> StackPanels

        private ObservableCollection<StackPanel> _engagedQueryStackPanels;

        public ObservableCollection<StackPanel> EngagedQueryStackPanels
        {
            get
            {
                if (_engagedQueryStackPanels == null)
                {
                    _engagedQueryStackPanels = new ObservableCollection<StackPanel>
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

        #region Queries control

        public ICommand GenerateModelCommand
        {
            get { return new RelayCommand(GenerateModel, CanDo); }
        }

        private void GenerateModel()
        {
            try
            {
                World = new FrontendLogic().PrepareWorld(this);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Wystąpił błąd podczas generowania modelu\n" + exception.Message, "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckModelGenerated()
        {
            if (World == null)
                throw new ApplicationException("Nie wygenerowano modelu");
        }

        private void CalculateExecutableQuery(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckModelGenerated();
                InputAggregator.PopulateViewModels(this);
                var executableQueriesViewModels = InputAggregator.ExecutableQueriesViewModels;
                var executableQueryIndex = FindExecutableQueryIndexByButton((Button) sender);
                var queryVM = executableQueriesViewModels[executableQueryIndex];

                new FrontendLogic().CalculateExecutableQuery(this, queryVM,
                    ExecutableQueryStackPanels[executableQueryIndex]);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Wystąpił błąd podczas obliczeń\n" + exception.Message, "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateAfterQuery(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckModelGenerated();
                InputAggregator.PopulateViewModels(this);

                var afterQueriesViewModels = InputAggregator.AfterQueriesViewModels;

                var afterQueryIndex = FindAfterQueryIndexByButton((Button)sender);
                var queryVM = afterQueriesViewModels[afterQueryIndex];
                new FrontendLogic().CalculateAfterQuery(this, queryVM, AfterQueryStackPanels[afterQueryIndex]);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Wystąpił błąd podczas obliczeń\n" + exception.Message, "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateEngagedQuery(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckModelGenerated();
                InputAggregator.PopulateViewModels(this);
                var engagedQueriesViewModels = InputAggregator.EngagedQueriesViewModels;
                var engagedQueryIndex = FindEngagedQueryIndexByButton((Button) sender);
                var queryVM = engagedQueriesViewModels[engagedQueryIndex];
                new FrontendLogic().CalculateEngagedQuery(this, queryVM, EngagedQueryStackPanels[engagedQueryIndex]);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Wystąpił błąd podczas obliczeń\n" + exception.Message, "Błąd",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}