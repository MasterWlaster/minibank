using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using Minibank.Core.Exceptions;
using Minibank.Data.Exchanges.Models;
using Minibank.Core.Exchanges;
using Minibank.Core.Helpers;
using Microsoft.Extensions.Configuration;

namespace Minibank.Data.Exchanges
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetRateAsync(string currencyCode, CancellationToken cancellationToken)
        {
            return await GetRateFromJsonAsync(currencyCode.Trim().ToUpper(), cancellationToken);
        }

        private async Task<decimal> GetRateFromJsonAsync(string currencyCode, CancellationToken cancellationToken)
        {
            if (currencyCode == Currency.DefaultCurrency)
            {
                return 1;
            }

            var response = await _httpClient
                .GetFromJsonAsync<ExchangeRateResponse>("daily_json.js", cancellationToken);

            if (response == null)
            {
                throw new ValidationException("empty response");
            }

            if (!response.Valute.ContainsKey(currencyCode))
            {
                throw new ValidationException("unknown currency code");
            }

            return response.Valute[currencyCode].Value;
        }
    }
}
