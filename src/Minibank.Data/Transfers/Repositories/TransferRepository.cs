using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Minibank.Core.Domains.Transfers;
using Minibank.Core.Domains.Transfers.Repositories;

namespace Minibank.Data.Transfers.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly Context _context;

        public TransferRepository(Context context)
        {
            _context = context;
        }

        public void Create(Transfer data)
        {
            var entity = new TransferDbModel()
            {
                Money = data.Money,
                CurrencyCode = data.CurrencyCode,
                FromAccountId = data.FromAccountId,
                ToAccountId = data.ToAccountId
            };

            _context.Transfers.Add(entity);
        }
    }
}
