using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Services
{
    public interface IAccountService
    {
        int Create(int userId, string currencyCode);
        void Close(int id);
        void ChangeMoney(int id, decimal delta);
        decimal CalculateCommission(decimal amount, int fromAccountId, int toAccountId);
        void DoTransfer(decimal amount, int fromAccountId, int toAccountId);
    }
}
