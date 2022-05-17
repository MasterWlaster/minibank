using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Repositories
{
    public interface IAccountRepository
    {
        void Create(int userId, string currencyCode);
        Task<Account> GetAsync(int id, CancellationToken cancellationToken);
        Task AddMoneyAsync(int id, decimal delta, CancellationToken cancellationToken);
        Task CloseAsync(int id, CancellationToken cancellationToken);
        Task<bool> IsActiveWithUserAsync(int userId, CancellationToken cancellationToken);
    }
}
