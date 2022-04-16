using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Repositories
{
    public interface IAccountRepository
    {
        void Create(int userId, string currencyCode);
        Task<Account> GetAsync(int id);
        Task AddMoneyAsync(int id, decimal delta);
        Task CloseAsync(int id);
        Task<bool> ExistsWithUserAsync(int userId);
    }
}
