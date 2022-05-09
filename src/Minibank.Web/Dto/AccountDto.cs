using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Web.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CurrencyCode { get; set; }
    }
}
