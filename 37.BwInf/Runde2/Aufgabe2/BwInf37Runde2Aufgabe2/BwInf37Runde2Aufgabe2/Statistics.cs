using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf37Runde2Aufgabe2
{
    class Statistics
    {
        private long[] numberOfCallsForGivenRecursionDepth;
        public Statistics(int size)
        {
            numberOfCallsForGivenRecursionDepth = new long[size + 1];
        }

        public void IncrementCounterForGivenRecursionDepth(int depth)
        {
            numberOfCallsForGivenRecursionDepth[depth]++;
        }

        public void PrintStatistics()
        {

        }
    }
}
