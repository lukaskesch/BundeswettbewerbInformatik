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
            Add("SophisticatedSolvers");
        }
    }
    public enum KindOfSolver
    {
        StupidSolver, AverageSolver, SophisticatedSolver, SophisticatedSolvers
    }

    abstract class Solver
    {
        protected Data data;
        protected Statistics statistics;
        private Stopwatch stopwatch = new Stopwatch();

        public Solver(Data AData)
        {
            data = AData;
        }

        protected void StartStopwatch()
        {
            stopwatch.Restart();
        }
        protected void StopStopwatchAndSaveElapsedTime()
        {
            stopwatch.Stop();
            data.ElapsedSeconds = (double)stopwatch.ElapsedMilliseconds / 1000;

        }

        public void Solve()
        {
            statistics = new Statistics(data);
            StartStopwatch();

            try
            {
                SetBeginningBricks();
                Backtracking(data.height + 1);
            }
            catch (Exception e)
            {
                StopStopwatchAndSaveElapsedTime();
                statistics.SaveStatistics();
                //MessageBox.Show(e.Message);
                return;
            }

            StopStopwatchAndSaveElapsedTime();
            MessageBox.Show("No solution could be found");
        }

        /// <summary>
        /// O(n) - Sets Data.height many bricks. Their width ranges from 1 to Data.height. Every brick is set in a new row
        /// </summary>
        private void SetBeginningBricks()
        {
            int JointPosition;
            for (int height = 0; height < data.height; height++)
            {
                JointPosition = height + 1;
                data.CurrentJointPosition[height] = JointPosition;
                data.UsedBricks[height, height] = true;
                data.Bricks[height, 0] = JointPosition;
                data.NumberOfBricksInGivenRow[height] = 1;
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

            for (int height = 0; height < data.height; height++)
            {
                currentRowJointPosition = data.CurrentJointPosition[height];
                neededBrickLength = recursionJointPosition - currentRowJointPosition;

                bool isBrickAvailabe = neededBrickLength <= data.NumberOfBricks;
                if (isBrickAvailabe)
                {
                    bool isBrickUnUsed = !data.UsedBricks[height, neededBrickLength - 1];
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
            int numberOfBricksInRow = data.NumberOfBricksInGivenRow[height];
            data.Bricks[height, numberOfBricksInRow] = length;
            data.UsedBricks[height, length - 1] = true;
            data.CurrentJointPosition[height] = recursionJointPosition;
            data.NumberOfBricksInGivenRow[height]++;
        }
        protected void RemoveBrickFromWall(Tuple<int, int> tuple, int recursionJointPosition)
        {
            int height = tuple.Item1;
            int length = tuple.Item2;
            int numberOfBricksInRow = --data.NumberOfBricksInGivenRow[height];
            data.Bricks[height, numberOfBricksInRow] = 0;
            data.UsedBricks[height, length - 1] = false;
            data.CurrentJointPosition[height] = recursionJointPosition - length;
        }
        protected bool CheckForInvalidRow()
        {
            return false;
        }
        protected bool CheckForValidSolution()
        {
            for (int height = 0; height < data.height; height++)
            {
                bool isUncloseableGap = (data.length - data.CurrentJointPosition[height]) > data.NumberOfBricks;
                if (isUncloseableGap)
                {
                    return false;
                }
            }
            return true;
        }
        protected void InsertMissingPieces()
        {
            for (int height = 0; height < data.height; height++)
            {
                int brickLength = data.length - data.CurrentJointPosition[height];
                if (brickLength > 0)
                    data.Bricks[height, data.NumberOfBricks - 1] = brickLength;
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

            for (int height = 0; height < data.height; height++)
            {
                currentRowJointPosition = data.CurrentJointPosition[height];
                neededBrickLength = recursionJointPosition - currentRowJointPosition;

                bool isBrickAvailabe = false;

                for (int brickLength = neededBrickLength; brickLength <= data.NumberOfBricks; brickLength++)
                {
                    bool isThisBrickAvailabe = !data.UsedBricks[height, brickLength - 1];
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
        public StupidSolver(Data AData) : base(AData) { }

        protected override void Backtracking(int recursionJointPosition)
        {
            //Check if end is reached and if so if a valid solution has been found
            if (recursionJointPosition > data.length && CheckForValidSolution())
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
        public AverageSolver(Data AData) : base(AData) { }

        protected override void Backtracking(int recursionJointPosition)
        {
            //Check if end is reached and if so if a valid solution has been found
            if (recursionJointPosition > data.length && CheckForValidSolution())
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
        public SophisticatedSolver(Data AData) : base(AData) { }

        protected override void Backtracking(int JointNumber)
        {
        }
    }
}
