using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BwInf36Runde2Aufgabe1
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
    public enum KindOfSolver
    {
        StupidSolver, AverageSolver, SophisticatedSolver
    }

    abstract class Solver
    {
        protected MainWindow mainWindow;
        protected Statistics statistics;
        private Stopwatch stopwatch = new Stopwatch();
        private string ElapsedTime;

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
            ElapsedTime = ElapsedSeconds.ToString();
            mainWindow.LabelElapsedTime.Content = ElapsedTime;
        }
        protected void PrintResult()
        {
            Drawing drawing = new Drawing(mainWindow);
            drawing.Draw();
        }


        public void Solve()
        {
            statistics = new Statistics(Data.length);
            StartStopwatch();

            try
            {
                SetBeginningBricks();
                Backtracking(Data.height + 1);
            }
            catch (Exception e)
            {
                StopStopwatchAndPrintElapsedTime();
                PrintResult();
                statistics.SaveStatistics(ElapsedTime);
                //MessageBox.Show(e.Message);
                return;
            }

            StopStopwatchAndPrintElapsedTime();
            MessageBox.Show("No solution could be found");
        }

        /// <summary>
        /// O(n) - Sets Data.height many bricks. Their width ranges from 1 to Data.height. Every brick is set in a new row
        /// </summary>
        private void SetBeginningBricks()
        {
            int JointPosition;
            for (int height = 0; height < Data.height; height++)
            {
                JointPosition = height + 1;
                Data.CurrentJointPosition[height] = JointPosition;
                Data.UsedBricks[height, height] = true;
                Data.Bricks[height, 0] = JointPosition;
                Data.NumberOfBricksInGivenRow[height] = 1;
            }
        }
        abstract protected void Backtracking(int jointNumber);

        /// <summary>
        /// O(n) - Returns a list of valid Bricks to set. The first value is the rowIndex and the second number is the brickLength 
        /// </summary>
        /// <param name="recursionJointPosition"></param>
        /// <returns></returns>
        protected List<Tuple<int, int>> GetNextValidBricks(int recursionJointPosition)
        {
            int currentRowJointPosition, neededBrickLength;
            List<Tuple<int, int>> ValidBricks = new List<Tuple<int, int>>();

            for (int height = 0; height < Data.height; height++)
            {
                currentRowJointPosition = Data.CurrentJointPosition[height];
                neededBrickLength = recursionJointPosition - currentRowJointPosition;

                bool isBrickAvailabe = neededBrickLength <= Data.NumberOfBricks;
                if (isBrickAvailabe)
                {
                    bool isBrickUnUsed = !Data.UsedBricks[height, neededBrickLength - 1];
                    if (isBrickUnUsed)
                    {
                        ValidBricks.Add(new Tuple<int, int>(height, neededBrickLength));
                    }
                }
            }

            return ValidBricks;
        }
        protected void AddBrickToWall(Tuple<int, int> tuple, int recursionJointPosition)
        {
            statistics.IncrementCounterForGivenRecursionDepth(recursionJointPosition);

            int height = tuple.Item1;
            int length = tuple.Item2;
            int numberOfBricksInRow = Data.NumberOfBricksInGivenRow[height];
            Data.Bricks[height, numberOfBricksInRow] = length;
            Data.UsedBricks[height, length - 1] = true;
            Data.CurrentJointPosition[height] = recursionJointPosition;
            Data.NumberOfBricksInGivenRow[height]++;
        }
        protected void RemoveBrickFromWall(Tuple<int, int> tuple, int recursionJointPosition)
        {
            int height = tuple.Item1;
            int length = tuple.Item2;
            int numberOfBricksInRow = --Data.NumberOfBricksInGivenRow[height];
            Data.Bricks[height, numberOfBricksInRow] = 0;
            Data.UsedBricks[height, length - 1] = false;
            Data.CurrentJointPosition[height] = recursionJointPosition - length;
        }
        protected bool CheckForInvalidRow()
        {
            return false;
        }
        protected bool CheckForValidSolution()
        {
            for (int height = 0; height < Data.height; height++)
            {
                bool isUncloseableGap = (Data.length - Data.CurrentJointPosition[height]) > Data.NumberOfBricks;
                if (isUncloseableGap)
                {
                    return false;
                }
            }
            return true;
        }
        protected void InsertMissingPieces()
        {
            for (int height = 0; height < Data.height; height++)
            {
                int brickLength = Data.length - Data.CurrentJointPosition[height];
                if (brickLength > 0)
                    Data.Bricks[height, Data.NumberOfBricks - 1] = brickLength;
            }
        }
        /// <summary>
        /// O(n^2) - 
        /// </summary>
        /// <param name="recursionJointPosition"></param>
        /// <returns></returns>
        protected bool CanContinuePruning(int recursionJointPosition)
        {
            int currentRowJointPosition, neededBrickLength;

            for (int height = 0; height < Data.height; height++)
            {
                currentRowJointPosition = Data.CurrentJointPosition[height];
                neededBrickLength = recursionJointPosition - currentRowJointPosition;

                bool isBrickAvailabe = false;

                for (int brickLength = neededBrickLength; brickLength <= Data.NumberOfBricks; brickLength++)
                {
                    bool isThisBrickAvailabe = !Data.UsedBricks[height, brickLength - 1];
                    if (isThisBrickAvailabe)
                    {
                        isBrickAvailabe = true;
                        break;
                    }
                }

                if (!isBrickAvailabe)
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Normal Depth-First-Search without pruning
    /// </summary>
    class StupidSolver : Solver
    {
        public StupidSolver(MainWindow AMainWindow) : base(AMainWindow) { }

        protected override void Backtracking(int recursionJointPosition)
        {
            //Check if end is reached and if so if a valid solution has been found
            if (recursionJointPosition > Data.length && CheckForValidSolution())
            {
                InsertMissingPieces();
                throw new FoundSolutionExeptions();
            }

            List<Tuple<int, int>> ValidBricks = GetNextValidBricks(recursionJointPosition);
            //ValidBricks.Shuffle();

            foreach (Tuple<int, int> tuple in ValidBricks)
            {
                AddBrickToWall(tuple, recursionJointPosition);

                Backtracking(recursionJointPosition + 1);

                RemoveBrickFromWall(tuple, recursionJointPosition);
            }
        }

    }

    /// <summary>
    /// Randomized Depth-First-Search with pruning l1 (continously checking for invalid rows)
    /// </summary>
    class AverageSolver : Solver
    {
        public AverageSolver(MainWindow AMainWindow) : base(AMainWindow) { }

        protected override void Backtracking(int recursionJointPosition)
        {
            //Check if end is reached and if so if a valid solution has been found
            if (recursionJointPosition > Data.length && CheckForValidSolution())
            {
                InsertMissingPieces();
                throw new FoundSolutionExeptions();
            }
            if (!CanContinuePruning(recursionJointPosition))
            {
                return;
            }
            List<Tuple<int, int>> ValidBricks = GetNextValidBricks(recursionJointPosition);
            //ValidBricks.Shuffle();

            foreach (Tuple<int, int> tuple in ValidBricks)
            {
                AddBrickToWall(tuple, recursionJointPosition);

                Backtracking(recursionJointPosition + 1);

                RemoveBrickFromWall(tuple, recursionJointPosition);
            }
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
