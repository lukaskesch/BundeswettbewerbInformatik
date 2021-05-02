using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BwInf37Runde2Aufgabe2
{
    /// <summary>
    /// Stores lists of all solvers
    /// </summary>
    public class Solvers : ObservableCollection<string>
    {
        public Solvers()
        {
            Add("StupidSolver");
            Add("AverageSolver");
            Add("SophisticatedSolver");
        }
    }

    abstract class Solver
    {
        protected MainWindow mainWindow;
        private Stopwatch stopwatch = new Stopwatch();

        public Solver(MainWindow AMainWindow)
        {
            mainWindow = AMainWindow;
        }

        protected void StartStopwatch()
        {
            stopwatch.Restart();
        }
        protected void StopStopwatchAndPrintElapsedTime()
        {
            stopwatch.Stop();
            double ElapsedSeconds = (double)stopwatch.ElapsedMilliseconds / 1000;
            mainWindow.LabelElapsedTime.Content = ElapsedSeconds.ToString();
        }
        protected void PrintResult()
        {
            Drawing drawing = new Drawing(mainWindow);
            drawing.Draw();
        }


        public void Solve()
        {
            StartStopwatch();

            try
            {
                Backtracking(0);
            }
            catch (Exception)
            {
                StopStopwatchAndPrintElapsedTime();
                PrintResult();
                return;
            }

            StopStopwatchAndPrintElapsedTime();
            MessageBox.Show("No solution could be found");
        }
        abstract protected void Backtracking(int JointNumber);

        protected bool CheckForInvalidRow()
        {
            return false;
        }
        protected bool CheckForValidSolution()
        {
            return false;
        }
    }

    /// <summary>
    /// Randomized Depth-First-Search without pruning
    /// </summary>
    class StupidSolver : Solver
    {
        public StupidSolver(MainWindow AMainWindow) : base(AMainWindow) { }

        protected override void Backtracking(int JointNumber)
        {
            MessageBox.Show("test45");
            return;

            //Check if end is reached and if so if a valid solution has been found
            if (JointNumber >= Data.length && CheckForValidSolution())
            {
                throw new FoundSolutionExeptions();
            }

            for (int height = 0; height < Data.height; height++)
            {
                int BrickLength = GetNextUnusedBrick(height);
                if (!CheckIfBrickIsValid(height, BrickLength))
                {
                    return;
                }

                //Add brick
                Data.Bricks[height, JointNumber] = BrickLength;
                Data.UsedBricks[height, BrickLength] = true;
                int JointIndex = Data.CurrentJointPosition[height] + BrickLength;
                Data.CurrentJointPosition[height] = JointIndex;
            }

            return;
        }


        private int GetNextUnusedBrick(int height)
        {
            for (int i = 0; i < Data.NumberOfBricks; i++)
            {
                bool BrickUsed = Data.UsedBricks[height, i];
                if (!BrickUsed)
                {
                    return i;
                }
            }
            return 0;
        }
        private bool CheckIfBrickIsValid(int height, int BrickLength)
        {
            return false;
        }
        private bool Pruning()
        {
            return true;
        }

    }

    /// <summary>
    /// Randomized Depth-First-Search with pruning l1 (continously checking for invalid rows)
    /// </summary>
    class AverageSolver : Solver
    {
        public AverageSolver(MainWindow AMainWindow) : base(AMainWindow) { }

        protected override void Backtracking(int JointNumber)
        {
        }
    }

    /// <summary>
    /// Randomized Depth-First-Search with pruning l2 (symetry)
    /// </summary>
    class SophisticatedSolver : Solver
    {
        public SophisticatedSolver(MainWindow AMainWindow) : base(AMainWindow) { }

        protected override void Backtracking(int JointNumber)
        {
        }
    }
}
