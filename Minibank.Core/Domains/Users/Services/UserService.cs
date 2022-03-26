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

        public UserService(IUserRepository userRepository, IAccountRepository accountService)
        {
            _userRepository = userRepository;
            _accountRepository = accountService;
        }

        public int Create(User data)
        {
            return _userRepository.Create(data);
        }

        public void Delete(int id)
        {
            if (_accountRepository.ExistsWithUser(id))
            {
                throw new ValidationException("user has active accounts");
            }

            _userRepository.Get(id);

            _userRepository.Delete(id);
        }

        public User Get(int id)
        {
            return _userRepository.Get(id);
        }

        public void Update(int id, User data)
        {
            _userRepository.Update(id, data);
        }
    }
}
