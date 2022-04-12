using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minibank.Data.Users;
using Microsoft.EntityFrameworkCore.Design;
using Minibank.Data.Accounts;
using Minibank.Data.Transfers;

namespace Minibank.Data
{
    public class Context : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<AccountDbModel> Accounts { get; set; }
        public DbSet<TransferDbModel> Transfers { get; set; }

        public Context(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
            //modelBuilder.ApplyConfiguration(new UserDbModel.Map());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class Factory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql("Host=localhost;Port=5432;Database=Minibank;Username=minibank;Password=123456")
                .Options;

            return new Context(options);
        }
    }
}


