using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        public int Create(User userInfo)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public User Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, User userInfo)
        {
            throw new NotImplementedException();
        }
    }
}
