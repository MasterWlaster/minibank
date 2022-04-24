using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Data.Users.Helpers;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }
        
        public void Create(User data, CancellationToken cancellationToken)
        {
            var entity = new UserDbModel()
            {
                Login = data.Login,
                Email = data.Email,
            };
            
            _context.Users.Add(entity);
        }

        public async Task<User> GetAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return entity == null ? null : MapperUserDb.ToUser(entity);
        }

        public async Task UpdateAsync(int id, User data, CancellationToken cancellationToken)
        {
            var entity = await 
                _context.Users.FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

            if (entity == null)
            {
                return;
            }

            entity.Login = data.Login ?? entity.Login;
            entity.Email = data.Email ?? entity.Email;

            _context.Users.Update(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

            if (entity == null)
            {
                return;
            }

            _context.Users.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(it => it.Id == id, cancellationToken);
        }
    }
}
