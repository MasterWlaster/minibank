using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core
{
    public interface ICurrencyConverter
    {
        /// <param name="from">Сurrency code to convert from</param>
        /// <param name="to">Currency code to convert into</param>
        float Convert(int value, string fromCurrency, string toCurrency);
    }
}
