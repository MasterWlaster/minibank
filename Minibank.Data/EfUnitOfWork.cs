using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core;

namespace Minibank.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public EfUnitOfWork(Context context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
