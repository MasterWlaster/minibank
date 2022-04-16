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
        private IDbContextTransaction transaction;

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
            transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (transaction == null)
            {
                throw new Exception("empty transaction");
            }

            await transaction.CommitAsync();

            DeleteTransaction();
        }

        public void DeleteTransaction()
        {
            transaction.Rollback();
            transaction.Dispose();
            transaction = null;
        }

        /*public async Task DoTransactionAsync(Task task)
        {
            await BeginTransactionAsync();

            try
            {
                task.Start();

                await CommitTransactionAsync();
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                DeleteTransaction();
            }
        }*/
    }
}
