using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Minibank.Web.Dto.Mapping;
using Minibank.Core.Domains.Accounts.Services;
using Microsoft.AspNetCore.Mvc;
using Minibank.Web.Dto;

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
        /// <param name="cancellationToken"></param>
        [HttpPost]
        public async Task Create(int userId, string currencyCode, CancellationToken cancellationToken)
        {
            await _accountService.CreateAsync(userId, currencyCode, cancellationToken);
        }

        /// <summary>
        /// Close
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        public async Task Close(int id, CancellationToken cancellationToken)
        {
            await _accountService.CloseAsync(id, cancellationToken);
        }

        /// <summary>
        /// Change Money
        /// </summary>
        /// <param name="id"></param>
        /// <param name="delta"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("change_money")]
        public async Task ChangeMoney(int id, decimal delta, CancellationToken cancellationToken)
        {
            await _accountService.AddMoneyAsync(id, delta, cancellationToken);
        }

        /// <summary>
        /// Calculate Commission
        /// </summary>
        /// <param name="money"></param>
        /// <param name="fromAccountId"></param>
        /// <param name="toAccountId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<decimal> CalculateCommission(decimal money, int fromAccountId, int toAccountId, CancellationToken cancellationToken)
        {
            return await _accountService.CalculateCommissionAsync(money, fromAccountId, toAccountId, cancellationToken);
        }

        /// <summary>
        /// Do Transfer
        /// </summary>
        /// <param name="money"></param>
        /// <param name="fromAccountId"></param>
        /// <param name="toAccountId"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost("do_transfer")]
        public async Task DoTransfer(decimal money, int fromAccountId, int toAccountId, CancellationToken cancellationToken)
        {
            await _accountService.DoTransferAsync(money, fromAccountId, toAccountId, cancellationToken);
        }
    }
}
