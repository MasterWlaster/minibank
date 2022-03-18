using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minibank.Web.Dto.Mapping;
using Minibank.Core.Domains.Accounts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public void Create(int userId, string currencyCode)
        {
            _accountService.Create(userId, currencyCode);
        }

        [HttpDelete]
        public void Close(int id)
        {
            _accountService.Close(id);
        }

        [HttpGet]
        public decimal CalculateCommission(decimal money, int fromAccountId, int toAccountId)
        {
            return _accountService.CalculateCommission(money, fromAccountId, toAccountId);
        }

        [HttpPost]
        public void DoTransfer(decimal money, int fromAccountId, int toAccountId)
        {
            _accountService.DoTransfer(money, fromAccountId, toAccountId);
        }
    }
}
