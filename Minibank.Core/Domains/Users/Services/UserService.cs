using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Accounts.Repositories;
using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<User> _userValidator;

        public UserService(IUserRepository userRepository, IAccountRepository accountService, IUnitOfWork unitOfWork, IValidator<User> userValidator)
        {
            _userRepository = userRepository;
            _accountRepository = accountService;
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
        }

        public async Task CreateAsync(User data, CancellationToken cancellationToken)
        {
            await _userValidator.ValidateAndThrowAsync(data, cancellationToken);
            
            _userRepository.Create(data);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(id, cancellationToken))
            {
                return;
            }

            if (await _accountRepository.IsActiveWithUserAsync(id, cancellationToken))
            {
                throw new ValidationException("user has active accounts");
            }

            await _userRepository.DeleteAsync(id, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<User> GetAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(id, cancellationToken);

            if (user == null)
            {
                throw new ValidationException("user not found");
            }

            return user;
        }

        public async Task UpdateAsync(int id, User data, CancellationToken cancellationToken)
        {
            if (!await _userRepository.ExistsAsync(id, cancellationToken))
            {
                throw new ValidationException("user not found");
            }

            await _userRepository.UpdateAsync(id, data, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
