using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Services
{
    public interface IAccountService
    {
        Task CreateAsync(int userId, string currencyCode);
        Task CloseAsync(int id);
        Task AddMoneyAsync(int id, decimal delta);
        Task<decimal> CalculateCommissionAsync(decimal amount, int fromAccountId, int toAccountId);
        Task DoTransferAsync(decimal amount, int fromAccountId, int toAccountId);
    }
}
