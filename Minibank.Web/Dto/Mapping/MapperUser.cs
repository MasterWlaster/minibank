using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minibank.Core.Domains.Users;

namespace Minibank.Web.Dto.Mapping
{
    public static class MapperUser
    {
        public static UserDto Unmap(User user)
        {
            return new()
            {
                Id = (int)user?.Id,
                Login = user?.Login,
                Email = user?.Email,
            };
        }

        public static User Map(UserDto user)
        {
            return new()
            {
                Id = (int)user?.Id,
                Login = user?.Login,
                Email = user?.Email,
            };
        }
    }
}
