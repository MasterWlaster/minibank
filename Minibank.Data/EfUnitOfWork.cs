using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Minibank.Core;
using Minibank.Core.Exceptions;

namespace Minibank.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        private IDbContextTransaction _transaction;

        public EfUnitOfWork(Context context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task DisposeTransactionAsync()
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
