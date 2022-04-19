using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Exchanges
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public CurrencyConverter(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async Task<decimal> ConvertAsync(decimal value, string fromCurrency, string intoCurrency)
        {
            if (value < 0)
            {
                throw new ValidationException("Cannot convert negative value");
            }

            return 
                value * 
                await _exchangeRateProvider.GetRateAsync(fromCurrency) /
                await _exchangeRateProvider.GetRateAsync(intoCurrency);
        }
    }
}
