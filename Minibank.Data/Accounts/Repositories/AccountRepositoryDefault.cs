using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Exceptions;
using Minibank.Core.Helpers;
using Minibank.Core.Domains.Users.Services;
using Minibank.Data.Accounts.Helpers;
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Repositories;

namespace Minibank.Data.Accounts.Repositories
{
    public class AccountRepositoryDefault : IAccountRepository
    {
        Dictionary<int, AccountDbModel> id2Account = new();
        Dictionary<int, List<int>> userId2AccountIds = new();
        int lastId = 0;

        public int Create(int userId, string currencyCode)
        {
            id2Account.Add(
                newId(),
                new() { 
                    Id = lastId,
                    UserId = userId,
                    CurrencyCode = currencyCode,
                    IsActive = true,
                    Money = 0,
                    OpenDate = DateTime.UtcNow,
                });

            if (!userId2AccountIds.ContainsKey(userId))
            {
                userId2AccountIds[userId] = new();
            }
            userId2AccountIds[userId].Add(lastId);

            return lastId;
        }

        public void Delete(int id)
        {
            var account = getModel(id);

            if (account.Money != 0)
            {
                throw new ValidationException("not zero balance");
            }

            id2Account.Remove(id);
            userId2AccountIds[account.UserId].Remove(id);
            
        }

        public Account Get(int id)
        {
            return MapperAccountDb.UnmapDb(getModel(id));
        }

        public void Update(int id, Account data)
        {
            var account = getModel(id);

            account.IsActive = data.IsActive;
            account.Money = data.Money;
            account.CloseDate = data.CloseDate;
        }

        int newId()
        {
            return ++lastId;
        }

        AccountDbModel getModel(int id)
        {
            AccountDbModel model;

            if (!id2Account.TryGetValue(id, out model))
            {
                throw new Exception("account not found");
            }

            return model;
        }
    }
}
