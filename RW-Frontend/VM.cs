using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            var ep = new Expander() {Name = "AgentsExpanderListBox", Header = "Agenci", VerticalAlignment = VerticalAlignment.Center};
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
            stackPanel.Children.Add(CreateLogicExpTextBox("alfaExp"));
            stackPanel.Children.Add(CreateIfLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piExp"));
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