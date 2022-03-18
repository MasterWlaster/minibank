using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Domains.Transfers.Repositories;

namespace Minibank.Core.Domains.Transfers.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;

        public TransferService(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public void Log(Transfer data)
        {
            _transferRepository.Create(data);
        }
    }
}
