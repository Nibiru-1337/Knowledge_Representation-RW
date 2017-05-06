using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private TextBox CreateTextBox()
        {
            var textBox = new TextBox() { Height = 25, FontSize = 14, Margin = new Thickness(5) };
            textBox.BorderBrush = System.Windows.Media.Brushes.Red;
            textBox.LostFocus += ValidateTextBox;            
            return textBox;
        }
        private void ValidateTextBox(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox) sender;
            if (string.IsNullOrEmpty(textBox.Text))
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
                Margin = new Thickness(5)
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
            var comboBox = new ComboBox() { Name = "ActionsComboBox", Margin = new Thickness(5) };
            comboBox.SelectedItem = String.Empty;
            comboBox.DropDownOpened += (s, e) =>
            {
                var actions = ActionsTextBoxes.Select(_ => _.Text).Where(_=>_ != String.Empty);
                actions = actions.Concat(new List<string>() {String.Empty});
                comboBox.ItemsSource = actions; 
            };
            return comboBox;
        }

        private ComboBox CreateAgentsComboBox()
        {
            var comboBox = new ComboBox() {Name="AgentsComboBox", Margin = new Thickness(5)};
            comboBox.SelectedItem = String.Empty;
            comboBox.DropDownOpened += (s, e) =>
            {
                var actions = AgentsTextBoxes.Select(_ => _.Text).Where(_ => _ != String.Empty);
                actions = actions.Concat(new List<string>() { String.Empty });
                comboBox.ItemsSource = actions;
            };
            return comboBox;
        }

        private Label CreateByLabel()
        {
            var label = new Label() {Margin = new Thickness(5), Content = "by"};
            return label;
        }

        private Label CreateIfLabel()
        {
            var label = new Label() { Margin = new Thickness(5), Content = "if" };
            return label;
        }

        private TextBox CreateLogicExpTextBox(string name)
        {
            var textBox = new TextBox() { Height = 25, Width = 130, FontSize = 14, Margin = new Thickness(5), Name = name};
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
            FluentsTextBoxes.Add(CreateTextBox());
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
                        CreateTextBox()
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
            ActionsTextBoxes.Add(CreateTextBox());
            ActionsRemoveButtons.Add(CreateRemoveButton("Action"));
        }

        private void RemoveActionButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            int removingItemIdx = ActionsRemoveButtons.IndexOf(clickedButton);
            ActionsRemoveButtons.RemoveAt(removingItemIdx);
            ActionsTextBoxes.RemoveAt(removingItemIdx);
        }

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
                        CreateTextBox()
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
            AgentsTextBoxes.Add(CreateTextBox());
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
                        CreateTextBox()
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

        #region Causes clause
        public ICommand AddCausesClauseCommand
        {
            get { return new RelayCommand(AddCausesClause, CanDo); }
        }

        private void AddCausesClause()
        {
            CausesClausesStackPanels.Add(CreateCausesClauseStackPanel());
            CausesClausesRemoveButtons.Add(CreateRemoveButton("Causes"));
        }

        private StackPanel CreateCausesClauseStackPanel()
        {
            //var textBox = new TextBox() { Height = 25, FontSize = 14, Margin = new Thickness(5) };
            //textBox.BorderBrush = System.Windows.Media.Brushes.Red;
            //textBox.LostFocus += ValidateTextBox;
            //return textBox;
            var stackPanel = new StackPanel() {Orientation = Orientation.Horizontal};
            stackPanel.Children.Add(CreateActionsComboBox());
            stackPanel.Children.Add(CreateByLabel());
            stackPanel.Children.Add(CreateAgentsComboBox());
            stackPanel.Children.Add(new Label() { Margin = new Thickness(5), Content = "causes" });
            stackPanel.Children.Add(CreateLogicExpTextBox("alfaExp"));
            stackPanel.Children.Add(CreateIfLabel());
            stackPanel.Children.Add(CreateLogicExpTextBox("piExp"));
            return stackPanel;
        }

        private void RemoveCausesClauseButtonClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int removingItemIdx = CausesClausesRemoveButtons.IndexOf(clickedButton);
            CausesClausesRemoveButtons.RemoveAt(removingItemIdx);
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

        #region CausesClauses -> RemoveButtons

        private ObservableCollection<Button> _causesClausesRemoveButtons;

        public ObservableCollection<Button> CausesClausesRemoveButtons
        {
            get
            {
                if (_causesClausesRemoveButtons == null)
                {
                    _causesClausesRemoveButtons = new ObservableCollection<Button>()
                    {
                        CreateRemoveButton("Causes")
                    };
                }
                return _causesClausesRemoveButtons;
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