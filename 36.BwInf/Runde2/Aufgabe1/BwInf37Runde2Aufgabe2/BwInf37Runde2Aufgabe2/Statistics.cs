using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf36Runde2Aufgabe1
{
    class Statistics
    {
        private Data data;
        private long[] numberOfCallsForGivenRecursionDepth;
        public Statistics(Data AData)
        {
            data = AData;

            int size = data.length;
            numberOfCallsForGivenRecursionDepth = new long[size + 1];
        }

        public void IncrementCounterForGivenRecursionDepth(int depth)
        {
            numberOfCallsForGivenRecursionDepth[depth]++;
        }

        public void SaveStatistics()
        {
            string content = GetStatisticsString();
            SaveFile(content);
        }
        private string GetStatisticsString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string overview = string.Format("Statistics file for {0} with n = {1}. Runtime of {2} s. Created at {3}.", data.kindOfSolver, data.NumberOfBricks, data.ElapsedSeconds.ToString(), DateTime.Now);
            stringBuilder.Append(overview);

            for (int i = 0; i < numberOfCallsForGivenRecursionDepth.Length; i++)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(string.Format("{0} , {1}", i, numberOfCallsForGivenRecursionDepth[i]));
            }

            return stringBuilder.ToString();
        }
        private void SaveFile(string content)
        {
            Directory.CreateDirectory("Statistics");
            string title = string.Format(@"Statistics\{0}-n={1}-#{2}-{3}.csv", data.kindOfSolver, data.NumberOfBricks, data.solverIndex, DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss"));

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
