using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RW_Frontend.Annotations;

namespace RW_Frontend
{
    internal class ActionVM :INotifyPropertyChanged
    {
        private string _actionName;
        internal VM Parent { get; set; }

        public string ActionName
        {
            get { return _actionName; }
            set
            {
                if (value == _actionName) return;
                _actionName = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteActionCommand
        {
            get { return new RelayCommand(DeleteAction, CanDo); }
        }

        private void DeleteAction()
        {
            Parent.RemoveAction(this);
        }

        private bool CanDo()
        {
            return true;
        }
        public static ActionVM Create(VM parentVm, string actionName = "")
        {
            var actionVm = new ActionVM {ActionName = actionName, Parent = parentVm};
            return actionVm;
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