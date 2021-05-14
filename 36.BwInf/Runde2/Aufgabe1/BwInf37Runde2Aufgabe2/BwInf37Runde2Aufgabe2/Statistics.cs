﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf36Runde2Aufgabe1
{
    class Statistics
    {
        public bool DrawStatistics;
        private long[] numberOfCallsForGivenRecursionDepth;
        public Statistics(int size)
        {
            numberOfCallsForGivenRecursionDepth = new long[size + 1];
        }

        public void IncrementCounterForGivenRecursionDepth(int depth)
        {
            numberOfCallsForGivenRecursionDepth[depth]++;
        }

        public void SaveStatistics(KindOfSolver kindOfSolver, string ElapsedTime)
        {
            string content = GetStatisticsString(kindOfSolver, ElapsedTime);
            SaveFile(content, kindOfSolver);
        }
        private string GetStatisticsString(KindOfSolver kindOfSolver, string ElapsedTime)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string overview = string.Format("Statistics file for {0} with n = {1}. Runtime of {2} s. Created at {3}.", kindOfSolver, Data.NumberOfBricks, ElapsedTime, DateTime.Now);
            stringBuilder.Append(overview);

            for (int i = 0; i < numberOfCallsForGivenRecursionDepth.Length; i++)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("{0} , {1}", i, numberOfCallsForGivenRecursionDepth[i]));
            }

            return stringBuilder.ToString();
        }
        private void SaveFile(string content, KindOfSolver kindOfSolver)
        {
            Directory.CreateDirectory("Statistics");
            string title = string.Format(@"Statistics\{0}-{1}.csv", kindOfSolver, DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss"));

            StreamWriter WriterStatistics = File.AppendText(title);
            try
            {
                WriterStatistics.WriteLine(content);
            }
            finally
            {
                WriterStatistics.Close();
            }

        }
    }
}