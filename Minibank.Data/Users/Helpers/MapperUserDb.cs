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
        public static UserDbModel MapDb(User user)
        {
            return new()
            {
                Id = (int) user?.Id,
                Login = user?.Login,
                Email = user?.Email,
            };
        }

        public static User UnmapDb(UserDbModel user)
        {
            return new()
            {
                Id = (int) user?.Id,
                Login = user?.Login,
                Email = user?.Email,
            };
        }
    }
}
