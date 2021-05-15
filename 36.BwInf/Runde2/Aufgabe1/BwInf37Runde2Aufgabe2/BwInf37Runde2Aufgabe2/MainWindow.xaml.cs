using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
        ProgramHandler programHandler;
        public MainWindow()
        {
            InitializeComponent();
            programHandler = new ProgramHandler(this);
        }

        private void ButtonCalculate_Click(object sender, RoutedEventArgs e)
        {
            programHandler.Start();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            programHandler.Stop();
        }
    }
}
