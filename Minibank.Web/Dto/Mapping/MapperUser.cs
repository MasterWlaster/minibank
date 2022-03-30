using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minibank.Core.Domains.Users;

namespace Minibank.Web.Dto.Mapping
{
    public static class MapperUser
    {
        public static UserDto ToUserDto(User user)
        {
            return new()
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
            };
        }

        public static User ToUser(UserDto user)
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
