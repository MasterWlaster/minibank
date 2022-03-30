using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Transfers.Repositories
{
    public interface ITransferRepository
    {
        int Create(Transfer data);
    }
}
