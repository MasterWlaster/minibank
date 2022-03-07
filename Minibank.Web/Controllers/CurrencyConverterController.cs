using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ICurrencyConverter _currencyConverter;

        public CurrencyConverterController(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        [HttpGet("RUB")]
        public float ConvertRubles(int value, string currencyCode)
        {
            return _currencyConverter.Convert(value, "RUB", currencyCode);
        }
    }
}
