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
        public ObservableCollection<ActionVM> Actions { get; set; }
        public ObservableCollection<string> Agents { get; set; }

        public ObservableCollection<string> Noninertial { get; set; }

        public ObservableCollection<string> AlwaysStatements { get; set; }
        public ObservableCollection<string> InitiallyStatements { get; set; }

        public ObservableCollection<string> AfterStatements { get; set; }
        public ObservableCollection<CausesVM> CausesStatements { get; set; }
        public ObservableCollection<string> ReleasesStatements { get; set; }

        public VM()
        {
            Fluents = new ObservableCollection<string>();
            Actions = new ObservableCollection<ActionVM>();
            Agents = new ObservableCollection<string>();
            Noninertial = new ObservableCollection<string>();
            AlwaysStatements = new ObservableCollection<string>();
            InitiallyStatements = new ObservableCollection<string>();
            AfterStatements = new ObservableCollection<string>();
            CausesStatements = new ObservableCollection<CausesVM>();
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
            var agentsCollection = new ObservableCollection<string>(agents);
            var vm = new VM
            {
                //trzeba zaktualizować przy zmianie reprezentacji zdań 
                Fluents = new ObservableCollection<string>(fluents),
                Agents = agentsCollection,
                Noninertial = new ObservableCollection<string>(noninertial),
                AlwaysStatements = new ObservableCollection<string>(always),
                InitiallyStatements = new ObservableCollection<string>(initially),
                AfterStatements = new ObservableCollection<string>(after),
                ReleasesStatements = new ObservableCollection<string>(releases)
            };
            var actionsCollection = new ObservableCollection<ActionVM>(actions.Select(s=>ActionVM.Create(vm, s)));
            vm.Actions = actionsCollection;
            var agentVms = new ObservableCollection<CausesVM>(causes.Select(s => CausesVM.Create(s, actionsCollection, agentsCollection, vm)));
            vm.CausesStatements = agentVms;
            return vm;
        }

        #region Components Control

        private bool CanDo()
        {
            return true;
        }

        #region Fluents

        #region TextBoxes

        public ICommand AddActionCommand
        {
            get { return new RelayCommand(AddAction, CanDo); }
        }
        public ICommand AddCausesCommand
        {
            get { return new RelayCommand(AddCauses, CanDo); }
        }
        public ICommand AddFluentCommand
        {
            get { return new RelayCommand(AddFluent, CanDo); }
        }

        public ICommand RemoveFluentTextBoxCommand
        {
            get { return new RelayCommand(RemoveFluentTextBox, CanDo); }
        }

        private void AddAction()
        {
            Actions.Add(ActionVM.Create(this));
        }

        private void AddCauses()
        {
            CausesStatements.Add(CausesVM.Create("", Actions, Agents, this));
        }

        private void AddFluent()
        {
            FluentsTextBoxes.Add(new TextBox() {Height = 25, FontSize = 14, Margin = new Thickness(5)});
            FluentsRemoveButtons.Add(new Button()
            {
                Height = 25,
                Width = 25,
                FontSize = 14,
                Content = "X",
                Margin = new Thickness(5)
            });
        }

        private void RemoveFluentTextBox()
        {
            if (FluentsTextBoxes.Count > 0)
            {
                FluentsTextBoxes.Remove(FluentsTextBoxes.Last());
            }
        }

        private ObservableCollection<TextBox> _fluentsTextBoxes;

        public ObservableCollection<TextBox> FluentsTextBoxes
        {
            get
            {
                if (_fluentsTextBoxes == null)
                {
                    _fluentsTextBoxes = new ObservableCollection<TextBox>()
                    {
                        new TextBox() {Height = 25, FontSize = 14, Margin = new Thickness(5)}
                    };
                }
                return _fluentsTextBoxes;
            }
        }

        #endregion

        #region RemoveButtons

        public ICommand RemoveFluentRemoveButtonCommand
        {
            get { return new RelayCommand(RemoveFluentRemoveButton, CanDo); }
        }

        private void RemoveFluentRemoveButton()
        {
            if (FluentsRemoveButtons.Count > 0)
            {
                FluentsRemoveButtons.Remove(FluentsRemoveButtons.Last());
            }
        }

        private ObservableCollection<Button> _fluentsRemoveButtons;

        public ObservableCollection<Button> FluentsRemoveButtons
        {
            get
            {
                if (_fluentsRemoveButtons == null)
                {
                    _fluentsRemoveButtons = new ObservableCollection<Button>()
                    {
                        new Button() {Height = 25, Width = 25, FontSize = 14, Content = "X", Margin = new Thickness(5)}
                    };
                }
                return _fluentsRemoveButtons;
            }
        }

        public void RemoveAction(ActionVM actionVM)
        {
            Actions.Remove(actionVM);
        }

        public void RemoveCauses(CausesVM causesVM)
        {
            CausesStatements.Remove(causesVM);
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