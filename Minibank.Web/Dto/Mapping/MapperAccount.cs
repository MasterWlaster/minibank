using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minibank.Core.Domains.Accounts;

namespace Minibank.Web.Dto.Mapping
{
    public static class MapperAccount
    {
        public static AccountDto Unmap(Account model)
        {
            return new()
            {
                Id = (int)model?.Id,
                UserId = (int)model?.UserId,
                CurrencyCode = model?.CurrencyCode,
            };
        }

        public static Account Map(AccountDto model)
        {
            return new()
            {
                Id = (int)model?.Id,
                UserId = (int)model?.UserId,
                CurrencyCode = model?.CurrencyCode,
            };
        }
    }
}
