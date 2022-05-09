using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Exchanges;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ICurrencyConverter _currencyConverter;

        public CurrencyConverterController(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        [HttpGet]
        public async Task<decimal> Convert(int amount, string fromCurrency, string toCurrency, CancellationToken cancellationToken)
        {
            return await _currencyConverter.ConvertAsync(amount, fromCurrency, toCurrency, cancellationToken);
        }
    }
}
