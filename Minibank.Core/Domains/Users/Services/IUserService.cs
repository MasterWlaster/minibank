using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task CreateAsync(User data);
        Task<User> GetAsync(int id);
        Task UpdateAsync(int id, User data);
        Task DeleteAsync(int id);
    }
}
