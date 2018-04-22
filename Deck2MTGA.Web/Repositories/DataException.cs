using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deck2MTGA.Web.Repositories
{
    public class DataException : Exception
    {
        public bool Fatal { get; set; }

        public DataException(bool isFatal = false) : base()
        {
            Fatal = isFatal;
        }

        public DataException(string message, bool isFatal = false) : base(message)
        {
            Fatal = isFatal;
        }

        public DataException(string message, Exception innerException, bool isFatal = false) : base(message, innerException)
        {
            Fatal = isFatal;
        }
    }
}
