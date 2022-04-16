using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public void Create(User data)
        {
            var entity = new UserDbModel()
            {
                Login = data.Login,
                Email = data.Email,
            };
            
            //await using var transaction = await _context.Database.BeginTransactionAsync();
            //
            //_context.Users.Add(entity);
            //
            //await _context.SaveChangesAsync();
            //
            //await transaction.CommitAsync();
            //
            //return entity.Id;

            _context.Users.Add(entity);
        }

        public async Task<User> GetAsync(int id)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : MapperUserDb.ToUser(entity);
        }

        public async Task UpdateAsync(int id, User data)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id);

            if (entity == null)
            {
                return;
            }

            entity.Login = data.Login ?? data.Login;
            entity.Email = data.Email ?? data.Email;

            _context.Users.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id);

            if (entity == null)
            {
                return;
            }

            _context.Users.Remove(entity);
        }
    }
}
