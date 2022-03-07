using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Exceptions;

namespace Minibank.Core
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public CurrencyConverter(IExchangeRateProvider excangeRateProvider)
        {
            _exchangeRateProvider = excangeRateProvider;
        }

        public float Convert(int value, string fromCurrency, string intoCurrency)
        {
            if (value < 0)
            {
                throw new UserFriendlyException("Cannot convert negative value");
            }

            return 
                value * 
                _exchangeRateProvider.RateOf(fromCurrency) / 
                _exchangeRateProvider.RateOf(intoCurrency);
        }
    }
}
