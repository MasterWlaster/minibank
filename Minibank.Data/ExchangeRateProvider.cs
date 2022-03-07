using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core;

namespace Minibank.Data
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly Random _random;

        public ExchangeRateProvider()
        {
            _random = new Random();
        }

        public float RateOf(string currencyCode)
        {
            switch (currencyCode)
            {
                case "RUB":
                    return 1;
                case "USD":
                    return _random.Next(30, 150);
            }

            throw new Exception("Unknown currency code");
        }
    }
}
