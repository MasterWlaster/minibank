using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Minibank.Core.Domains.Users.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Login).NotEmpty().WithMessage("is empty");
            RuleFor(x => x.Email).NotEmpty().WithMessage("is empty");
        }
    }
}
