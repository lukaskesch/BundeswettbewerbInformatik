using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf36Runde2Aufgabe1
{
    public static class Data
    {
        public static bool OddNumberOfBricks;
        public static int NumberOfBricks;
        public static int height;
        public static int length;

        ///// <summary>
        ///// Stores if the ith joint is free
        ///// </summary>
        //public static bool[] FreeJoints;

        /// <summary>
        /// Stores the width of the brick in the ith row in the jth slot
        /// </summary>
        public static int[,] Bricks;

        /// <summary>
        /// Stores if a brick of length j+1 is used in the ith row
        /// </summary>
        public static bool[,] UsedBricks;

        /// <summary>
        /// Stores the current Joint position in the ith row
        /// </summary>
        public static int[] CurrentJointPosition;

        /// <summary>
        /// Stores the number of bricks in the ith row
        /// </summary>
        public static int[] NumberOfBricksInGivenRow;

        public static bool ReadInput(string input)
        {
            try
            {
                NumberOfBricks = int.Parse(input);
            }
            catch (Exception)
            {
                return false;
            }

            bool OutOfBounds = (NumberOfBricks < 3) || (NumberOfBricks > 120);
            if (OutOfBounds)
            {
                return false;
            }


            OddNumberOfBricks = NumberOfBricks % 2 != 0;
            if (OddNumberOfBricks)
            {
                NumberOfBricks--;
            }

            PrepareDatastructures();
            return true;
        }

        private static void PrepareDatastructures()
        {
            height = (NumberOfBricks + 2) / 2;
            length = (NumberOfBricks * (NumberOfBricks + 1)) / 2;

            //FreeJoints = new bool[length];
            CurrentJointPosition = new int[height];
            NumberOfBricksInGivenRow = new int[height];
            Bricks = new int[height, NumberOfBricks];
            UsedBricks = new bool[height, NumberOfBricks];

        }

    }
}
