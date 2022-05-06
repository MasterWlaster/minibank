using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        
        public void Create(int userId, string currencyCode)
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
        }

        public async Task<Account> GetAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

            return entity == null ? null : MapperAccountDb.ToAccount(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

            if (entity == null)
            {
                return;
            }

            _context.Accounts.Remove(entity);
        }

        public async Task AddMoneyAsync(int id, decimal delta, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

            if (entity == null)
            {
                return;
            }

            entity.Money += delta;

            _context.Accounts.Update(entity);
        }

        public async Task CloseAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

            if (entity == null)
            {
                return;
            }

            entity.IsActive = false;
            entity.CloseDate = DateTime.UtcNow;

            _context.Accounts.Update(entity);
        }

        public async Task<bool> IsActiveWithUserAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Accounts
                .AnyAsync(it => it.UserId == userId && it.IsActive, cancellationToken);
        }
    }
}
