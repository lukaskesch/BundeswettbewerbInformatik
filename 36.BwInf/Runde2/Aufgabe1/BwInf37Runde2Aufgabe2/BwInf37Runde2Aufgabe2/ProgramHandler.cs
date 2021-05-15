
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;

namespace BwInf36Runde2Aufgabe1
{
    public class ProgramHandler
    {
        private DispatcherTimer dispatcherTimer;
        private MetaData metaData;


        public ProgramHandler(MainWindow mainWindow)
        {
            metaData = new MetaData(mainWindow);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            Process.Start(System.AppDomain.CurrentDomain.BaseDirectory);
        }
        public void Start()
        {
            metaData.Reset();
            ReadInput();
            PrepareDatastructures();
            LockUI();
            Calculate();
            dispatcherTimer.Start();
        }
        private void Restart()
        {
            metaData.epoch++;
            metaData.Reset();
            PrepareDatastructures();
            Calculate();
            dispatcherTimer.Start();
        }
        public void Stop()
        {
            metaData.logger.Print("User requested stop");
            dispatcherTimer.Stop();
            AbortAllThreads();
            UnlockUI();
            metaData.logger.Stop();
        }
        private void LockUI()
        {
            metaData.mainWindow.ButtonCalculate.Visibility = Visibility.Collapsed;
            metaData.mainWindow.ButtonStop.Visibility = Visibility.Visible;
        }
        private void UnlockUI()
        {
            metaData.mainWindow.ButtonStop.Visibility = Visibility.Collapsed;
            metaData.mainWindow.ButtonCalculate.Visibility = Visibility.Visible;
        }
        private void ReadInput()
        {
            int NumberOfBricks;
            try
            {
                NumberOfBricks = int.Parse(metaData.mainWindow.TextBoxInput.Text);

                bool OutOfBounds = (NumberOfBricks < 3) || (NumberOfBricks > 120);
                if (OutOfBounds)
                {
                    throw new IndexOutOfRangeException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter an integer between 3 and 120", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int index = metaData.mainWindow.ComboBoxSolvers.SelectedIndex;
            switch (index)
            {
                case 0:
                    metaData.kindOfSolver = KindOfSolver.StupidSolver;
                    break;
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

            metaData.input = NumberOfBricks;
            metaData.logger.Print(string.Format("Requested {0} for n = {1}", metaData.kindOfSolver, metaData.input));


        }
        private void PrepareDatastructures()
        {

            switch (metaData.kindOfSolver)
            {
                case KindOfSolver.StupidSolver:
                    metaData.threads.Add(new CalculationThread(1, metaData.input, metaData.kindOfSolver, new ParameterizedThreadStart(RunStupidSolver)));
                    break;
                case KindOfSolver.AverageSolver:
                    metaData.threads.Add(new CalculationThread(1, metaData.input, metaData.kindOfSolver, new ParameterizedThreadStart(RunAverageSolver)));
                    break;
                case KindOfSolver.SophisticatedSolver:
                    metaData.threads.Add(new CalculationThread(1, metaData.input, metaData.kindOfSolver, new ParameterizedThreadStart(RunSophisticatedSolver)));
                    break;
                case KindOfSolver.SophisticatedSolvers:
                    int length = Environment.ProcessorCount;
                    if (length > 1) { length--; }
                    if (length > 4) { length--; }
                    if (length > 8) { length--; }
                    for (int i = 0; i < length; i++)
                    {
                        metaData.threads.Add(new CalculationThread(i + 1, metaData.input, metaData.kindOfSolver, new ParameterizedThreadStart(RunSophisticatedSolver)));
                    }
                    break;
                default:
                    return;
            }
        }
        private void Calculate()
        {
            int threadNumber = 1;
            foreach (CalculationThread calculationThread in metaData.threads)
            {
                metaData.logger.Print(string.Format("Thread #{0} started", threadNumber++));
                calculationThread.Start();
            }
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
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if ((metaData.tick++) % 5 == 0)
            {
                metaData.logger.Print(string.Format("(n={0}, epoch={1}) Tick #{2} ", metaData.input, metaData.epoch, metaData.tick - 1));
            }

            int indexThread = GetFinsishedThreads();
            if (indexThread >= 0)
            {
                ThreadFinished(indexThread);
                return;
            }

            if (metaData.tick >= metaData.maxTick)
            {
                RuntimeExceeded();
                return;
            }
        }
        private void ThreadFinished(int indexThread)
        {
            dispatcherTimer.Stop();
            AbortAllUnfinishedThreads();
            metaData.solutionIndex = indexThread;
            PrintOutput();
            UnlockUI();

            if ((bool)metaData.mainWindow.CheckBoxContinue.IsChecked)
            {
                metaData.mainWindow.TextBoxInput.Text = (metaData.input + 2).ToString();
                Start();
            }
            else
            {
                metaData.logger.Stop();
            }
        }
        private void RuntimeExceeded()
        {
            metaData.logger.Print(string.Format("Executiontime of {0} ticks exceeded", metaData.maxTick));
            metaData.logger.Print(string.Format("Stopping all {0} threads", metaData.threads.Count));
            AbortAllThreads();
            dispatcherTimer.Stop();
            if (metaData.kindOfSolver == KindOfSolver.SophisticatedSolver || metaData.kindOfSolver == KindOfSolver.SophisticatedSolvers)
            {
                Restart();
            }
        }
        private int GetFinsishedThreads()
        {
            int index = 0;
            foreach (CalculationThread calculationThread in metaData.threads)
            {
                if (!calculationThread.thread.IsAlive)
                {
                    metaData.logger.Print(string.Format("Thread #{0} finished", index + 1));
                    return index;
                }
                index++;
            }
            return -1;
        }
        private void AbortAllUnfinishedThreads()
        {
            int index = 1;
            foreach (CalculationThread calculationThread in metaData.threads)
            {
                if (calculationThread.thread.IsAlive)
                {
                    calculationThread.Abort();
                    metaData.logger.Print(string.Format("Thread #{0} stopped", index));
                    index++;
                }
            }
        }
        private void AbortAllThreads()
        {
            int index = 1;
            foreach (CalculationThread calculationThread in metaData.threads)
            {
                calculationThread.Abort();
                metaData.logger.Print(string.Format("Thread #{0} stopped", index));
                index++;
            }
        }
        private void PrintOutput()
        {
            metaData.logger.Print("Printing solution");

            string runtime = "Runtime of " + metaData.threads[metaData.solutionIndex].data.ElapsedSeconds.ToString() + "s";
            metaData.mainWindow.LabelElapsedTime.Content = runtime;

            Drawing drawing = new Drawing(metaData.mainWindow, metaData.threads[metaData.solutionIndex].data);
            drawing.Draw();
        }
    }
}

