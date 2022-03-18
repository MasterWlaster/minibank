using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Data.Users.Helpers;
using Minibank.Core.Exceptions;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepositoryDefault : IUserRepository
    {
        Dictionary<int, UserDbModel> id2User = new();
        int lastId = 0;
        
        public int Create(User userInfo)
        {
            userInfo.Id = newId();
            id2User[userInfo.Id] = MapperUserDb.MapDb(userInfo);

            return userInfo.Id;
        }

        public void Delete(int id)
        {
            if (!id2User.Remove(id))
            {
                throw new ValidationException("user not found");
            }
        }

        public User Get(int id)
        {
            return MapperUserDb.UnmapDb(getModel(id));
        }

        public void Update(int id, User userInfo)
        {
            var user = getModel(id);
            var userData = MapperUserDb.MapDb(userInfo);

            user.Login = userData?.Login ?? user?.Login;
            user.Email = userData?.Email ?? user?.Email;
        }

        int newId()
        {
            return ++lastId;
        }

        UserDbModel getModel(int id)
        {
            UserDbModel user;

            if (!id2User.TryGetValue(id, out user))
            {
                throw new ValidationException("user not found");
            }

            return user;
        }
    }
}
