using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Transfers.Repositories
{
    public interface ITransferRepository
    {
        void Create(Transfer data);
    }
}
