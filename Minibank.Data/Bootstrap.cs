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
using Minibank.Data.Exchanges;
using System.Net.Http;

namespace Minibank.Data
{
    public static class Bootstrap
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepositoryDefault>();
            services.AddSingleton<ITransferRepository, TransferRepositoryDefault>();
            services.AddSingleton<IAccountRepository, AccountRepositoryDefault>();
            services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            
            return services;
        }
    }
}
