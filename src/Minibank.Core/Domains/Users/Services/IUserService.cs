using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task CreateAsync(User data, CancellationToken cancellationToken);
        Task<User> GetAsync(int id, CancellationToken cancellationToken);
        Task UpdateAsync(int id, User data, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
