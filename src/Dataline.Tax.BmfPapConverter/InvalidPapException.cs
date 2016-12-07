using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter
{
    public class InvalidPapException : Exception
    {
        public InvalidPapException(string message) : base($"Der PAP ist ungültig: {message}")
        {
        }
    }
}
