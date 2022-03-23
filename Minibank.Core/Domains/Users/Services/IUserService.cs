using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        int Create(User data);
        User Get(int id);
        void Update(int id, User data);
        void Delete(int id);
    }
}
