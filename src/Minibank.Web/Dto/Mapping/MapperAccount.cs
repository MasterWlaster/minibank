using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minibank.Core.Domains.Accounts;

namespace Minibank.Web.Dto.Mapping
{
    public static class MapperAccount
    {
        public static AccountDto ToAccountDto(Account model)
        {
            return new()
            {
                Id = model.Id,
                UserId = model.UserId,
                CurrencyCode = model.CurrencyCode,
            };
        }

        public static Account ToAccount(AccountDto model)
        {
            return new()
            {
                Id = model.Id,
                UserId = model.UserId,
                CurrencyCode = model.CurrencyCode,
            };
        }
    }
}
