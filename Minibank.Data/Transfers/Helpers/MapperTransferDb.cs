using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Domains.Transfers;

namespace Minibank.Data.Transfers.Helpers
{
    public static class MapperTransferDb
    {
        public static TransferDbModel MapDb(Transfer model)
        {
            return new()
            {
                Id = model.Id,
                Money = model.Money,
                CurrencyCode = model.CurrencyCode,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId,
            };
        }

        public static Transfer UnmapDb(TransferDbModel model)
        {
            return new()
            {
                Id = model.Id,
                Money = model.Money,
                CurrencyCode = model.CurrencyCode,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId,
            };
        }
    }
}
