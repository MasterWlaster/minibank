using FluentValidation;

namespace Minibank.Core.Domains.Accounts.Validators
{
    public class AccountValidator : AbstractValidator<Account>
    {
        public AccountValidator()
        {
            RuleFor(x => x.CurrencyCode).NotEmpty().WithMessage("is empty");
        }
    }
}