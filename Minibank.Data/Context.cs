using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minibank.Data.Users;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
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
            
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSnakeCaseNamingConvention()
                .LogTo(Console.WriteLine);
            
            base.OnConfiguring(optionsBuilder);
        }
    }

    //for creating migrations
    public class Factory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql()
                .Options;

            return new Context(options);
        }
    }
}


