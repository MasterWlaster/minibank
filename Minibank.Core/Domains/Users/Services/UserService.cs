using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IAccountRepository accountService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _accountRepository = accountService;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(User data)
        {
            _userRepository.Create(data);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (await _accountRepository.ExistsWithUserAsync(id))
            {
                throw new ValidationException("user has active accounts");
            }

            await _userRepository.DeleteAsync(id);
            
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await _userRepository.GetAsync(id);
        }

        public async Task UpdateAsync(int id, User data)
        {
            await _userRepository.UpdateAsync(id, data);
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
