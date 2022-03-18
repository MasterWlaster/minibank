using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Helpers
{
    public static class Currency
    {
        public static string Normalize(string currencyCode)
        {
            return currencyCode.Trim().ToUpper();
        }
    }
}
