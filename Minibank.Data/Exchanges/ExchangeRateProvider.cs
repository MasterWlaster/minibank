using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Minibank.Core.Exceptions;
using Minibank.Data.Exchanges.Models;
using Minibank.Core.Exchanges;

namespace Minibank.Data.Exchanges
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExchangeRateProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public decimal RateOf(string currencyCode)
        {
            return GetRate(currencyCode.Trim().ToUpper());
        }

        decimal GetRate(string currencyCode)
        {
            if (currencyCode == "RUB")
            {
                return 1;
            }
            
            var httpClient = _httpClientFactory.CreateClient();
            var response = httpClient
                .GetFromJsonAsync<ExchangeRateResponse>("https://www.cbr-xml-daily.ru/daily_json.js")
                .GetAwaiter()
                .GetResult();

            if (response.Valute.ContainsKey(currencyCode))
            {
                return response.Valute[currencyCode].Value;
            }

            throw new ValidationException("Unknown currency code");
        }
    }
}
