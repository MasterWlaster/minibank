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
    public class UserRepository : IUserRepository
    {
        static Dictionary<int, UserDbModel> id2User = new();
        static int lastId = 0;
        
        public int Create(User data)
        {
            int id = NewId();

            data.Id = id;
            id2User[id] = MapperUserDb.ToUserDbModel(data);

            return id;
        }

        public void Delete(int id)
        {
            id2User.Remove(id);
        }

        public User Get(int id)
        {
            return MapperUserDb.ToUser(GetModel(id));
        }

        public void Update(int id, User data)
        {
            var user = GetModel(id);

            user.Login = data.Login ?? user.Login;
            user.Email = data.Email ?? user.Email;
        }

        int NewId()
        {
            return ++lastId;
        }

        UserDbModel GetModel(int id)
        {
            UserDbModel model;

            if (!id2User.TryGetValue(id, out model))
            {
                throw new ValidationException("user not found");
            }

            return model;
        }
    }
}
