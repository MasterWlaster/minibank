using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Data.Transfers.Helpers;
using Minibank.Core.Domains.Transfers;
using Minibank.Core.Domains.Transfers.Repositories;

namespace Minibank.Data.Transfers.Repositories
{
    public class TransferRepository2 : ITransferRepository
    {
        static Dictionary<int, TransferDbModel> id2transfer = new();
        static int lastId = 0;
        
        public int Create(Transfer data)
        {
            int id = NewId();

            data.Id = id;
            id2transfer[id] = MapperTransferDb.ToTransferDbModel(data);

            return id;
        }

        int NewId()
        {
            return ++lastId;
        }
    }
}
