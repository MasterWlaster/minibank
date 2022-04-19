using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Minibank.Core.Domains.Transfers.Validators
{
    /*public class TransferValidator : ITransferValidator
    {
        public AbstractValidator<Transfer> OnCreate => _onCreate;
        
        private readonly OnCreate _onCreate;

        public TransferValidator(OnCreate onCreate)
        {
            _onCreate = onCreate;
        }
    }

    public class OnCreate : AbstractValidator<Transfer>
    {
        public OnCreate()
        {
            RuleFor(x => x.CurrencyCode).NotEmpty().WithMessage("is empty");
        }
    }*/
}
