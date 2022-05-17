using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Exchanges
{
    public interface IExchangeRateProvider
    {
        Task<decimal> GetRateAsync(string currencyCode, CancellationToken cancellationToken);
    }
}
