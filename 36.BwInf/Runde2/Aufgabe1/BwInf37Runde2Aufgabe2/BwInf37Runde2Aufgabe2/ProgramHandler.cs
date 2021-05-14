
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;

namespace BwInf36Runde2Aufgabe1
{
    public class ProgramHandler
    {
        private MainWindow mainWindow;
        public ProgramHandler(MainWindow AMainWindow)
        {
            mainWindow = AMainWindow;
        }
        public void Start()
        {
            ReadInput();

            int numberOfCores = Environment.ProcessorCount;

            Logger logger = new Logger();
            logger.Start();
            logger.Print("test");

            ThreadStart threadStart = new ThreadStart(Calculate);
            Thread thread = new Thread(threadStart);
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
            while (thread.IsAlive)
            {

            }

            //thread.Join();
            //Calculate();


            //Thread.Sleep(15000);
            PrintOutput();
        }
        public void Calculate()
        {
            {
                StupidSolver stupidSolver = new StupidSolver();
                stupidSolver.Solve();
            }
        }
        public void ReadInput()
        {
            bool ValidInput = Data.ReadInput(mainWindow.TextBoxInput.Text);
            if (!ValidInput)
            {
                MessageBox.Show("Please enter an integer between 3 and 120", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int index = mainWindow.ComboBoxSolvers.SelectedIndex;
            switch (index)
            {
                case 0:
                    Data.kindOfSolver = KindOfSolver.StupidSolver;


                    return;
                case 1:
                    Data.kindOfSolver = KindOfSolver.AverageSolver;
                    AverageSolver averageSolver = new AverageSolver();
                    averageSolver.Solve();
                    break;
                case 2:
                    Data.kindOfSolver = KindOfSolver.SophisticatedSolver;
                    SophisticatedSolver sophisticatedSolver = new SophisticatedSolver();
                    sophisticatedSolver.Solve();
                    break;
                default:
                    MessageBox.Show("Please select a solver", "Action required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
            }
        }
        public void PrintOutput()
        {
            mainWindow.LabelElapsedTime.Content = Data.ElapsedSeconds.ToString();

            Drawing drawing = new Drawing(mainWindow);
            drawing.Draw();
        }
    }
}

