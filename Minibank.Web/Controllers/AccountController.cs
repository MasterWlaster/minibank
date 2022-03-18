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

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currencyCode"></param>
        [HttpPost]
        public void Create(int userId, string currencyCode)
        {
            _accountService.Create(userId, currencyCode);
        }

        /// <summary>
        /// Close
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public void Close(int id)
        {
            _accountService.Close(id);
        }

        /// <summary>
        /// Calculate Commission
        /// </summary>
        /// <param name="money"></param>
        /// <param name="fromAccountId"></param>
        /// <param name="toAccountId"></param>
        /// <returns></returns>
        [HttpGet]
        public decimal CalculateCommission(decimal money, int fromAccountId, int toAccountId)
        {
            return _accountService.CalculateCommission(money, fromAccountId, toAccountId);
        }

        /// <summary>
        /// Do Transfer
        /// </summary>
        /// <param name="money"></param>
        /// <param name="fromAccountId"></param>
        /// <param name="toAccountId"></param>
        [HttpPut]
        public void DoTransfer(decimal money, int fromAccountId, int toAccountId)
        {
            _accountService.DoTransfer(money, fromAccountId, toAccountId);
        }
    }
}
