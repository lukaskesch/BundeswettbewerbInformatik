using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BwInf36Runde2Aufgabe1
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

        private void ButtonBerechnen_Click(object sender, RoutedEventArgs e)
        {
            bool ValidInput = Data.ReadInput(TextBoxInput.Text);
            if (!ValidInput)
            {
                MessageBox.Show("Please enter an integer between 3 and 120", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int index = ComboBoxSolvers.SelectedIndex;
            switch (index)
            {
                case 0:
                    StupidSolver stupidSolver = new StupidSolver(this);
                    stupidSolver.Solve();
                    return;
                case 1:
                    AverageSolver normalSolver = new AverageSolver(this);
                    normalSolver.Solve();
                    break;
                case 2:
                    SophisticatedSolver sophisticatedSolver = new SophisticatedSolver(this);
                    sophisticatedSolver.Solve();
                    break;
                default:
                    MessageBox.Show("Please select a solver", "Action required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
            }
        }
    }
}
