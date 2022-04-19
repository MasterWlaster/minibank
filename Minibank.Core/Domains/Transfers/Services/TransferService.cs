using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domains.Transfers.Repositories;
using Minibank.Core.Domains.Transfers.Validators;

namespace Minibank.Core.Domains.Transfers.Services
{
    /*public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransferValidator _validator;
        //private readonly IValidator<Transfer> _validator;
        //private readonly IEnumerable<IValidator<Transfer>> _validators;

        public TransferService(ITransferRepository transferRepository, IUnitOfWork unitOfWork, ITransferValidator validator)
        {
            _transferRepository = transferRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task LogAsync(Transfer data)
        {
            //await _validators
            //    .FirstOrDefault(x => x.GetType() == typeof(TransferValidator))
            //    .ValidateAndThrowAsync(data);

            await _validator.OnCreate.ValidateAndThrowAsync(data);

            _transferRepository.Create(data);
            
            await _unitOfWork.SaveChangesAsync();
        }
    }*/
}
