using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Transfers.Services
{
    public interface ITransferService
    {
        void Log(Transfer data);
    }
}
