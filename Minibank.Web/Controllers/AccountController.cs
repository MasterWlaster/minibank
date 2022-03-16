using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public void Create(int userId, string currencyCode)
        {

        }

        [HttpDelete]
        public void Close(int id)
        {

        }

        [HttpGet]
        public decimal CalculateCommission(decimal money, int fromAccountId, int toAccountId)
        {
            return 0;
        }

        [HttpPost]
        public void Transfer(decimal money, int fromAccountId, int toAccountId)
        {

        }
    }
}
