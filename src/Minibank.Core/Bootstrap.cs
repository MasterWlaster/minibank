using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.Accounts.Services;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Exchanges;
using Minibank.Core.Helpers;

namespace Minibank.Core
{
    public static class Bootstrap
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICurrencyConverter, CurrencyConverter>();
            services.AddScoped<ICurrencyTool, Currency>();

            services.AddFluentValidation().AddValidatorsFromAssembly(typeof(Bootstrap).Assembly);

            return services;
        }
    }
}
