using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Repositories
{
    public interface IAccountRepository
    {
        int Create(int userId, string currencyCode);
        void Update(int id, Account data);
        Account Get(int id);
        void Delete(int id);
    }
}
