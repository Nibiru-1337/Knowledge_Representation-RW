using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
