using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Data.Exchanges.Models
{
    public class ExchangeRateResponse
    {
        public Dictionary<string, CurrencyInfo> Valute { get; set; }
    }

    public class CurrencyInfo
    {
        public decimal Value { get; set; }
    }
}
