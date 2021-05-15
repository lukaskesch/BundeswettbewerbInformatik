using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BwInf36Runde2Aufgabe1
{
    public class MetaData
    {
        public int input;
        public int tick;
        public int epoch;
        public readonly int maxTick;
        public int solutionIndex;
        public Logger logger;
        public MainWindow mainWindow;
        public KindOfSolver kindOfSolver;

        public List<CalculationThread> threads;

        public MetaData(MainWindow AMainWindow)
        {
            epoch = 0;
            tick = 0;
            maxTick = 120;
            solutionIndex = -1;
            mainWindow = AMainWindow;
            threads = new List<CalculationThread>();

            logger = new Logger();
            logger.Start();
        }
        public void Reset()
        {
            threads.Clear();
            tick = 0;
            solutionIndex = -1;
        }
    }
    public struct CalculationThread
    {
        public readonly Thread thread;
        public Data data;
        public CalculationThread(int id, int input, KindOfSolver kindOfSolver, ParameterizedThreadStart threadStart)
        {
            data = new Data(input, kindOfSolver, id);
            thread = new Thread(threadStart);
            thread.Priority = ThreadPriority.Highest;
        }
        public void Start()
        {
            thread.Start(data);
        }
        public void Abort()
        {
            thread.Abort();
        }
    }
    public class Data
    {
        public KindOfSolver kindOfSolver;
        public int solverIndex;
        public double ElapsedSeconds;
        public bool OddNumberOfBricks;
        public int NumberOfBricks;
        public int height;
        public int length;

        /// <summary>
        /// Stores the width of the brick in the ith row in the jth slot
        /// </summary>
        public int[,] Bricks;

        /// <summary>
        /// Stores if a brick of length j+1 is used in the ith row
        /// </summary>
        public bool[,] UsedBricks;

        /// <summary>
        /// Stores the current Joint position in the ith row
        /// </summary>
        public int[] CurrentJointPosition;

        /// <summary>
        /// Stores the number of bricks in the ith row
        /// </summary>
        public int[] NumberOfBricksInGivenRow;

        public Data(int ANumberOfBricks, KindOfSolver AKindOfSolver, int ASolverIndex)
        {
            solverIndex = ASolverIndex;
            NumberOfBricks = ANumberOfBricks;
            PrepareDatastructures();
            kindOfSolver = AKindOfSolver;
        }

        private void PrepareDatastructures()
        {
            OddNumberOfBricks = NumberOfBricks % 2 != 0;
            if (OddNumberOfBricks)
            {
                NumberOfBricks--;
            }

            height = (NumberOfBricks + 2) / 2;
            length = (NumberOfBricks * (NumberOfBricks + 1)) / 2;

            CurrentJointPosition = new int[height];
            NumberOfBricksInGivenRow = new int[height];
            Bricks = new int[height, NumberOfBricks];
            UsedBricks = new bool[height, NumberOfBricks];

        }

    }
}
