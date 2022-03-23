using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Domains.Accounts;

namespace Minibank.Data.Accounts.Helpers
{
    public static class MapperAccountDb
    {
        public static AccountDbModel ToAccountDbModel(Account model)
        {
            return new()
            {
                Id = model.Id,
                UserId = model.UserId,
                Money = model.Money,
                CurrencyCode = model.CurrencyCode,
                IsActive = model.IsActive,
                OpenDate = model.OpenDate,
                CloseDate = model.CloseDate,
            };
        }

        public static Account ToAccount(AccountDbModel model)
        {
            return new()
            {
                Id = model.Id,
                UserId = model.UserId,
                Money = model.Money,
                CurrencyCode = model.CurrencyCode,
                IsActive = model.IsActive,
                OpenDate = model.OpenDate,
                CloseDate = model.CloseDate,
            };
        }
    }
}
