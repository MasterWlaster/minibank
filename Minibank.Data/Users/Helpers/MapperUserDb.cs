using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Domains.Users;

namespace Minibank.Data.Users.Helpers
{
    public static class MapperUserDb
    {
        public static UserDbModel ToUserDbModel(User user)
        {
            return new()
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
            };
        }

        public static User ToUser(UserDbModel user)
        {
            return new()
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
            };
        }
    }
}
