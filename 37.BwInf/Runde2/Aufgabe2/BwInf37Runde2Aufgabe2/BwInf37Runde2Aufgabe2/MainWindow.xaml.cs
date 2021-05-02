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

namespace BwInf37Runde2Aufgabe2
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
            bool rightInput = Data.ReadInput(TextBoxInput.Text);
            if (!rightInput)
            {
                MessageBox.Show("Please enter an integer between 3 and 40", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Drawing drawing = new Drawing(this);
            drawing.Draw();


            int index = ComboBoxSolvers.SelectedIndex;
            switch (index)
            {
                case 0:
                    StupidSolver stupidSolver = new StupidSolver(this);
                    stupidSolver.Solve();
                    return;
                case 1:
                    NormalSolver normalSolver = new NormalSolver(this);
                    normalSolver.Solve();
                    break;
                default:
                    MessageBox.Show("Please select a solver");
                    return;
            }
        }
    }
}
