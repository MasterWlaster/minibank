using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Helpers
{
    public class Currency : ICurrencyTool
    {
        private static readonly List<string> currencies = new(){ "RUB", "USD", "EUR"};

        public string DefaultCurrency => "RUB";
        
        public string Normalize(string currencyCode)
        {
            return currencyCode.Trim().ToUpper();
        }

        public bool IsValid(string currencyCode)
        {
            currencyCode = Validate(currencyCode);

            return currencyCode != null;
        }
        
        public string Validate(string currencyCode)
        {
            currencyCode = Normalize(currencyCode);

            return !currencies.Contains(currencyCode) ? null : currencyCode;
        }
    }
}
