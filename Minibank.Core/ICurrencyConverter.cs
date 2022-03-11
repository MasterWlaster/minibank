using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core
{
    public interface ICurrencyConverter
    {
        /// <param name="fromCurrency">Сurrency code to convert from</param>
        /// <param name="intoCurrency">Currency code to convert into</param>
        decimal Convert(int value, string fromCurrency, string intoCurrency);
    }
}
