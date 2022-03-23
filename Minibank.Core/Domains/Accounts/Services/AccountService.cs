using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minibank.Core.Exceptions;
using Minibank.Core.Helpers;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Transfers.Services;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Exchanges;

namespace Minibank.Core.Domains.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransferService _transferService;
        private readonly IUserService _userService;
        private readonly ICurrencyConverter _currencyConverter;

        const decimal COMMISSION_MULTIPLIER = 0.02m;
        const int DECIMAL_PLACES = 2;

        public AccountService(IAccountRepository accountRepository, ITransferService transferService, IUserService userService, ICurrencyConverter currencyConverter)
        {
            _accountRepository = accountRepository;
            _transferService = transferService;
            _userService = userService;
            _currencyConverter = currencyConverter;
        }

        public decimal CalculateCommission(decimal amount, int fromAccountId, int toAccountId)
        {
            if (fromAccountId == toAccountId)
            {
                throw new ValidationException("accounts are equals");
            }

            var fromAccount = _accountRepository.Get(fromAccountId);
            var toAccount = _accountRepository.Get(toAccountId);
            
            if (fromAccount.IsActive && toAccount.IsActive)
            {
                if (fromAccount.UserId == toAccount.UserId)
                {
                    return 0;
                }
                return Decimal.Round(amount * COMMISSION_MULTIPLIER, DECIMAL_PLACES);
            }

            throw new Exception("accounts not active");
        }

        public void Close(int id)
        {
            _accountRepository.Delete(id);
        }

        public void ChangeMoney(int id, decimal amount)
        {
            _accountRepository.ChangeMoney(id, amount);
        }

        public int Create(int userId, string currencyCode)
        {
            _userService.Get(userId);

            currencyCode = Currency.Validate(currencyCode);

            if (currencyCode == null)
            {
                throw new ValidationException("invalid currency");
            }

            return _accountRepository.Create(userId, currencyCode);
        }

        public void DoTransfer(decimal amount, int fromAccountId, int toAccountId)
        {
            _accountRepository.ChangeMoney(fromAccountId, -amount);

            var commission = CalculateCommission(amount, fromAccountId, toAccountId);
            var fromAccount = _accountRepository.Get(toAccountId);
            var toAccount = _accountRepository.Get(toAccountId);
            
            amount = _currencyConverter.Convert(amount - commission, fromAccount.CurrencyCode, toAccount.CurrencyCode);

            _accountRepository.ChangeMoney(toAccountId, amount);

            _transferService.Log(
                new() {
                    Money = toAccount.Money + amount - commission,
                    CurrencyCode = fromAccount.CurrencyCode,
                    FromAccountId = fromAccountId,
                    ToAccountId = toAccountId,
                });
        }
    }
}
