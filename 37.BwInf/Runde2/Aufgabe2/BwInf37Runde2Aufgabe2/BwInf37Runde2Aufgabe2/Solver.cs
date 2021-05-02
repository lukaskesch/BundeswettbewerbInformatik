using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BwInf37Runde2Aufgabe2
{
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
            stopwatch.Start();
        }
        protected void StopStopwatchAndPrintElapsedTime()
        {
            stopwatch.Stop();
            double ElapsedSeconds = (double)stopwatch.ElapsedMilliseconds / 1000;
            mainWindow.LabelElapsedTime.Content = ElapsedSeconds.ToString();
        }
        protected void ShowResult()
        {
            Drawing drawing = new Drawing(mainWindow);
            drawing.Draw();
        }
    }

    class HorizontalJointSolverStupid : Solver
    {
        public HorizontalJointSolverStupid(MainWindow AMainWindow) : base(AMainWindow)
        {

        }

        public void Solve()
        {
            StartStopwatch();

            MessageBox.Show("Test");
            try
            {
                throw new FoundSolutionExeptions();
            }
            catch (Exception)
            {
                MessageBox.Show("Test");
                throw;
            }

            StopStopwatchAndPrintElapsedTime();
            ShowResult();
        }

        private bool Backtracking(int JointNumber)
        {
            //Check if end is reached and if so if a valid solution has been found
            if (JointNumber >= Data.length && CheckForValidSolution())
            {
                return true;
            }

            for (int height = 0; height < Data.height; height++)
            {
                int BrickLength = GetNextUnusedBrick(height);
                if (!CheckIfBrickIsValid(height, BrickLength))
                {
                    return false;
                }

                //Add brick
                Data.Bricks[height, JointNumber] = BrickLength;
                Data.UsedBricks[height, BrickLength] = true;
                int JointIndex = Data.CurrentJointPosition[height] + BrickLength;
                Data.CurrentJointPosition[height] = JointIndex;
            }

            return false;
        }

        private bool CheckForValidSolution()
        {
            return false;
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
            int JointIndex = Data.CurrentJointPosition[height] + BrickLength;

            if (Data.FreeJoints[JointIndex])
            {
                return false;
            }
            return true;
        }
        private bool Pruning()
        {
            return true;
        }

    }

    class VerticalSolver : Solver
    {
        public VerticalSolver(MainWindow AMainWindow) : base(AMainWindow)
        {

        }
    }
}
