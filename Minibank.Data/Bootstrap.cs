using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Exchanges;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Transfers.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Data.Accounts.Repositories;
using Minibank.Data.Transfers.Repositories;
using Minibank.Data.Users.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Minibank.Data.Exchanges;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net.Http;

namespace Minibank.Data
{
    public static class Bootstrap
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            
            services.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>(
                client => client.BaseAddress = new Uri(configuration["ExchangesCbRussia"]));
            
            services.AddDbContext<Context>(
                options => options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(configuration["DbConnectionString"]));
            
            return services;
        }
    }
}
