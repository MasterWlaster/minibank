using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Exceptions
{
    public class VisibleInResponseException : Exception
    {
        public VisibleInResponseException(string message) : base(message)
        {
            
        }
    }
}
