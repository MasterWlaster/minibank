using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Services
{
    public interface IAccountService
    {
        Task CreateAsync(int userId, string currencyCode, CancellationToken cancellationToken);
        Task CloseAsync(int id, CancellationToken cancellationToken);
        Task AddMoneyAsync(int id, decimal delta, CancellationToken cancellationToken);
        Task<decimal> CalculateCommissionAsync(decimal amount, int fromAccountId, int toAccountId, CancellationToken cancellationToken);
        Task DoTransferAsync(decimal amount, int fromAccountId, int toAccountId, CancellationToken cancellationToken);
    }
}
