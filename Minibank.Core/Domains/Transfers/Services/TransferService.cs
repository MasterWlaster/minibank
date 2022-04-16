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
        private readonly IUnitOfWork _unitOfWork;

        public TransferService(ITransferRepository transferRepository, IUnitOfWork unitOfWork)
        {
            _transferRepository = transferRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task LogAsync(Transfer data)
        {
            _transferRepository.Create(data);
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
