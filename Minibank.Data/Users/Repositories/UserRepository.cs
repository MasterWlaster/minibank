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
        Dictionary<int, UserDbModel> id2User = new();
        int lastId = 0;
        
        public int Create(User data)
        {
            int id = NewId();

            data.Id = id;
            id2User[id] = MapperUserDb.MapDb(data);

            return id;
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
            return MapperUserDb.UnmapDb(GetModel(id));
        }

        public void Update(int id, User data)
        {
            var user = GetModel(id);
            var userData = MapperUserDb.MapDb(data);

            user.Login = userData.Login;
            user.Email = userData.Email;
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
