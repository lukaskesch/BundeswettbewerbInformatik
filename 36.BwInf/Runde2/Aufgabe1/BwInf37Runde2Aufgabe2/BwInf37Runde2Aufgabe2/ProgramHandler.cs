
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
        private MetaData metaData;
        private Logger logger;

        public ProgramHandler(MainWindow AMainWindow)
        {
            mainWindow = AMainWindow;
            metaData = new MetaData();
            logger = new Logger();

            logger.Start();
            logger.Print("test");
        }
        public void Start()
        {
            ReadInput();
            PrepareDatastructures();
            Calculate();
            PrintOutput();
        }
        public void RunStupidSolver(object obj)
        {
            Data data = (Data)obj;
            StupidSolver stupidSolver = new StupidSolver(data);
            stupidSolver.Solve();
        }
        public void RunAverageSolver(object obj)
        {
            Data data = (Data)obj;
            AverageSolver averageSolver = new AverageSolver(data);
            averageSolver.Solve();
        }
        public void RunSophisticatedSolver(object obj)
        {
            Data data = (Data)obj;
            SophisticatedSolver sophisticatedSolver = new SophisticatedSolver(data);
            sophisticatedSolver.Solve();
        }

        public void ReadInput()
        {
            int NumberOfBricks;
            try
            {
                NumberOfBricks = int.Parse(mainWindow.TextBoxInput.Text);

                bool OutOfBounds = (NumberOfBricks < 3) || (NumberOfBricks > 120);
                if (OutOfBounds)
                {
                    throw new System.IndexOutOfRangeException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter an integer between 3 and 120", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            metaData.input = NumberOfBricks;

            int index = mainWindow.ComboBoxSolvers.SelectedIndex;
            switch (index)
            {
                case 0:
                    metaData.kindOfSolver = KindOfSolver.StupidSolver;
                    return;
                case 1:
                    metaData.kindOfSolver = KindOfSolver.AverageSolver;
                    break;
                case 2:
                    metaData.kindOfSolver = KindOfSolver.SophisticatedSolver;
                    break;
                case 3:
                    metaData.kindOfSolver = KindOfSolver.SophisticatedSolvers;
                    break;
                default:
                    MessageBox.Show("Please select a solver", "Action required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
            }
        }
        private void PrepareDatastructures()
        {
            switch (metaData.kindOfSolver)
            {
                case KindOfSolver.StupidSolver:
                    metaData.Threads.Add(new CalculationThread(metaData.input, metaData.kindOfSolver, new ParameterizedThreadStart(RunStupidSolver)));
                    break;
                case KindOfSolver.AverageSolver:
                    metaData.Threads.Add(new CalculationThread(metaData.input, metaData.kindOfSolver, new ParameterizedThreadStart(RunAverageSolver)));
                    break;
                case KindOfSolver.SophisticatedSolver:
                    metaData.Threads.Add(new CalculationThread(metaData.input, metaData.kindOfSolver, new ParameterizedThreadStart(RunSophisticatedSolver)));
                    break;
                case KindOfSolver.SophisticatedSolvers:
                    int length = Environment.ProcessorCount;
                    if (length > 1) { length--; }
                    for (int i = 0; i < length; i++)
                    {
                        metaData.Threads.Add(new CalculationThread(metaData.input, metaData.kindOfSolver, new ParameterizedThreadStart(RunSophisticatedSolver)));
                    }
                    break;
                default:
                    return;
            }
        }
        private void Calculate()
        {
            foreach (CalculationThread calculationThread in metaData.Threads)
            {
                calculationThread.Start();
            }

            //metaData.Threads[0].Start();
            while (metaData.Threads[0].thread.IsAlive)
            {

            }
        }
        public void PrintOutput()
        {
            mainWindow.LabelElapsedTime.Content = metaData.Threads[0].data.ElapsedSeconds.ToString();

            Drawing drawing = new Drawing(mainWindow, metaData.Threads[0].data);
            drawing.Draw();
        }
    }
}

