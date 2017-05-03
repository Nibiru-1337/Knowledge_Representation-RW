using System;
using System.Windows;

namespace RW_Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            new FrontendLogic().SetDataContext(this);
        }
    }
}
