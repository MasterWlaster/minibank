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
        
        public int Create(User data)
        {
            var entity = new UserDbModel()
            {
                Login = data.Login,
                Email = data.Email,
            };

            _context.Users.Add(entity);

            return 0; //todo return id
        }

        public User Get(int id)
        {
            var entity = _context.Users
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            return entity == null ? null : MapperUserDb.ToUser(entity);
        }

        public void Update(int id, User data)
        {
            var entity = _context.Users.FirstOrDefault(it => it.Id == id);

            if (entity == null)
            {
                return;
            }

            entity.Login = data.Login;
            entity.Email = data.Email;

            //todo updating
            //_context.Users.Update(entity);
        }

        public void Delete(int id)
        {
            var entity = _context.Users.FirstOrDefault(it => it.Id == id);

            if (entity == null)
            {
                return;
            }

            _context.Users.Remove(entity);
        }
    }
}
