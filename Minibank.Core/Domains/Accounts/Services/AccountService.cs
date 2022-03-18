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
                return Decimal.Round(amount * 0.02m, 2);
            }

            throw new Exception("accounts not active");
        }

        public void Close(int id)
        {
            _accountRepository.Delete(id);
        }

        public int Create(int userId, string currencyCode)
        {
            _userService.Get(userId);

            var currency = Currency.Normalize(currencyCode);

            if (currency != "RUB" && currency != "USD" && currency != "EUR")
            {
                throw new ValidationException("invalid currency");
            }

            return _accountRepository.Create(userId, currencyCode);
        }

        public void DoTransfer(decimal amount, int fromAccountId, int toAccountId)
        {
            var commission = CalculateCommission(amount, fromAccountId, toAccountId);
            var fromAccount = _accountRepository.Get(fromAccountId);
            var toAccount = _accountRepository.Get(toAccountId);

            if (fromAccount.Money - amount < 0)
            {
                throw new ValidationException("not enough money");
            }

            _accountRepository.Update(
                fromAccount.Id,
                new() { Money = fromAccount.Money - amount });

            commission = _currencyConverter.Convert(commission, fromAccount.CurrencyCode, toAccount.CurrencyCode);
            amount = _currencyConverter.Convert(amount, fromAccount.CurrencyCode, toAccount.CurrencyCode);

            _accountRepository.Update(
                toAccount.Id,
                new() { Money = toAccount.Money + amount - commission });

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
