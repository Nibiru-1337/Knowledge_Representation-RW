using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            return new TextBox() {Height = 25, FontSize = 14, Margin = new Thickness(5)};
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
            }
            return button;
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