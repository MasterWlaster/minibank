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
using Minibank.Core.Helpers;
using Microsoft.Extensions.Configuration;

namespace Minibank.Data.Exchanges
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ExchangeRateProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public decimal RateOf(string currencyCode)
        {
            return GetRate(currencyCode.Trim().ToUpper());
        }

        decimal GetRate(string currencyCode)
        {
            if (currencyCode == Currency.DefaultCurrency)
            {
                return 1;
            }
            
            var httpClient = _httpClientFactory.CreateClient();
            var response = httpClient
                .GetFromJsonAsync<ExchangeRateResponse>("daily_json.js")//_configuration["ExchangesCbRussia"] + "/daily_json.js") // "")
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
