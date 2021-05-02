using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BwInf37Runde2Aufgabe2
{
    [Serializable]
    public class FoundSolutionExeptions : Exception
    {
        // Constructors
        public FoundSolutionExeptions()
        { }

        // Ensure Exception is Serializable
        protected FoundSolutionExeptions(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        { }
    }
}
