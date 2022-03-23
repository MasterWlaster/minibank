﻿using System;
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
    public class AccountRepository : IAccountRepository
    {
        static Dictionary<int, AccountDbModel> id2Account = new();
        static Dictionary<int, List<int>> userId2AccountIds = new();
        static int lastId = 0;

        public int Create(int userId, string currencyCode)
        {
            int id = NewId();

            id2Account.Add(
                id,
                new() { 
                    Id = id,
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
            userId2AccountIds[userId].Add(id);

            return id;
        }

        public void Delete(int id)
        {
            var account = GetModel(id);

            if (account.Money != 0)
            {
                throw new ValidationException("not zero balance");
            }

            id2Account.Remove(id);
            userId2AccountIds[account.UserId].Remove(id);
            
        }

        public Account Get(int id)
        {
            return MapperAccountDb.UnmapDb(GetModel(id));
        }

        public void ChangeMoney(int id, decimal delta)
        {
            var model = GetModel(id);

            if (!model.IsActive)
            {
                throw new ValidationException("account not active");
            }

            if (model.Money + delta < 0)
            {
                throw new ValidationException("balance cannot be lower than zero");
            }

            model.Money += delta;
        }

        public bool ExistsWithUser(int userId)
        {
            var list = userId2AccountIds[userId];

            return list != null && list.Count > 0;
        }

        int NewId()
        {
            return ++lastId;
        }

        AccountDbModel GetModel(int id)
        {
            AccountDbModel model;

            if (!id2Account.TryGetValue(id, out model))
            {
                throw new ValidationException("account not found");
            }

            return model;
        }
    }
}
