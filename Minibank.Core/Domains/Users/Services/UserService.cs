using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Domains.Users.Repositories;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int Create(User userInfo)
        {
            return _userRepository.Create(userInfo);
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id); //todo: check if accounts exist, throw ValidationExceptions
        }

        public User Get(int id)
        {
            return _userRepository.Get(id);
        }

        public void Update(int id, User userInfo)
        {
            _userRepository.Update(id, userInfo);
        }
    }
}
