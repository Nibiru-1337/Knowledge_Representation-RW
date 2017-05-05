using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RW_Frontend.Annotations;

namespace RW_Frontend
{
    internal class CausesVM : INotifyPropertyChanged
    {
        private ObservableCollection<string> _agents;
        private string _efects;
        private string _conditions;
        public string Action { get; set; }
        public ObservableCollection<ActionVM> Actions { get; set; }

        public ObservableCollection<string> Agents
        {
            get { return _agents; }
            set
            {
                if (Equals(value, _agents)) return;
                _agents = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<object> SelectedAgents { get; set; }

        public string Efects
        {
            get { return _efects; }
            set
            {
                if (value == _efects) return;
                _efects = value;
                OnPropertyChanged();
            }
        }

        public string Conditions
        {
            get { return _conditions; }
            set
            {
                if (value == _conditions) return;
                _conditions = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteStatementCommand
        {
            get { return new RelayCommand(DeleteStatement, CanDo); }
        }

        private void DeleteStatement()
        {
            Parent.RemoveCauses(this);
        }

        private bool CanDo()
        {
            return true;
        }

        internal VM Parent { get; set; }

        public static CausesVM Create(string arg, ObservableCollection<ActionVM> actionsCollection, ObservableCollection<string> agentsCollection, VM parentVm)
        {
            var causesVm = new CausesVM
            {
                Actions = actionsCollection,
                Agents = agentsCollection,
                Parent = parentVm
            };
            return causesVm;
        }
        
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