using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Minibank.Data.Exchanges.Models;
using Minibank.Core;

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
            return rateOf(currencyCode.Trim().ToUpper());
        }

        private decimal rateOf(string currencyCode)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = httpClient
                .GetFromJsonAsync<ExchangeRateResponse>("https://www.cbr-xml-daily.ru/daily_json.js")
                .GetAwaiter()
                .GetResult();

            if (response.Code2Info.ContainsKey(currencyCode))
            {
                return response.Code2Info[currencyCode].Value;
            }

            throw new Exception("Unknown currency code");
        }
    }
}
