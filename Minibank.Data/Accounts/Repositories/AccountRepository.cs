using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Data.Accounts.Helpers;

namespace Minibank.Data.Accounts.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Context _context;

        public AccountRepository(Context context)
        {
            _context = context;
        }
        
        public int Create(int userId, string currencyCode)
        {
            var entity = new AccountDbModel()
            {
                UserId = userId,
                CurrencyCode = currencyCode,
                Money = 0,
                IsActive = true,
                OpenDate = DateTime.UtcNow,
            };

            _context.Accounts.Add(entity);

            return 0; //todo return id
        }

        public Account Get(int id)
        {
            var entity = _context.Accounts
                .AsNoTracking()
                .FirstOrDefault(it => it.Id == id);

            return entity == null ? null : MapperAccountDb.ToAccount(entity);
        }

        public void Delete(int id)
        {
            var entity = _context.Accounts.FirstOrDefault(it => it.Id == id);

            if (entity == null)
            {
                return;
            }

            _context.Accounts.Remove(entity);
        }

        public void Update(int id, Account data, bool isMoneyUpdating = false)
        {
            var entity = _context.Accounts.FirstOrDefault(it => it.Id == id);

            if (entity == null)
            {
                return;
            }

            if (isMoneyUpdating)
            {
                entity.Money = data.Money;
            }

            //todo updating
            //_context.Accounts.Update(entity);
        }

        public bool ExistsWithUser(int userId)
        {
            var entity = _context.Accounts.FirstOrDefault(it => it.UserId == userId);

            return entity != null;
        }
    }
}
