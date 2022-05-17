using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Minibank.Core.Exceptions;
using Minibank.Core.Helpers;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Transfers;
using Minibank.Core.Domains.Transfers.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Exchanges;

namespace Minibank.Core.Domains.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransferRepository _transferRepository;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrencyTool _currencyTool;
        private readonly IUserRepository _userRepository;

        private const decimal CommissionMultiplier = 0.02m;
        private const int DecimalPlaces = 2;

        public AccountService(IAccountRepository accountRepository, ITransferRepository transferRepository, ICurrencyConverter currencyConverter, IUnitOfWork unitOfWork, ICurrencyTool currencyTool, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _transferRepository = transferRepository;
            _currencyConverter = currencyConverter;
            _unitOfWork = unitOfWork;
            _currencyTool = currencyTool;
            _userRepository = userRepository;
        }

        public async Task<decimal> CalculateCommissionAsync(decimal amount, int fromAccountId, int toAccountId, CancellationToken cancellationToken)
        {
            if (fromAccountId == toAccountId)
            {
                throw new ValidationException("accounts are equals");
            }

            var fromAccount = await _accountRepository.GetAsync(fromAccountId, cancellationToken);
            var toAccount = await _accountRepository.GetAsync(toAccountId, cancellationToken);

            if (fromAccount == null || toAccount == null)
            {
                throw new ValidationException("accounts not found");
            }

            if (!fromAccount.IsActive || !toAccount.IsActive)
            {
                throw new ValidationException("not active accounts");
            }

            return fromAccount.UserId == toAccount.UserId
                ? 0
                : decimal.Round(amount * CommissionMultiplier, DecimalPlaces);
        }

        public async Task CloseAsync(int id, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAsync(id, cancellationToken);

            if (account == null)
            {
                return;
            }

            if (!account.IsActive)
            {
                return;
            }

            if (account.Money != 0)
            {
                throw new ValidationException("not 0 balance");
            }

            await _accountRepository.CloseAsync(id, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddMoneyAsync(int id, decimal delta, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAsync(id, cancellationToken);

            if (account == null)
            {
                throw new ValidationException("account not found");
            }

            if (!account.IsActive)
            {
                throw new ValidationException("account not active");
            }

            if (account.Money + delta < 0)
            {
                throw new ValidationException("balance cannot be lower than 0");
            }

            await _accountRepository.AddMoneyAsync(id, delta, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CreateAsync(int userId, string currencyCode, CancellationToken cancellationToken)
        {
            currencyCode = _currencyTool.Validate(currencyCode);

            if (currencyCode == null)
            {
                throw new ValidationException("invalid currency");
            }

            if (!await _userRepository.ExistsAsync(userId, CancellationToken.None))
            {
                throw new ValidationException("user not found");
            }

            _accountRepository.Create(userId, currencyCode);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DoTransferAsync(decimal amount, int fromAccountId, int toAccountId, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await AddMoneyAsync(fromAccountId, -amount, cancellationToken);

                var commission = await CalculateCommissionAsync(amount, fromAccountId, toAccountId, cancellationToken);

                var fromAccount = await _accountRepository.GetAsync(fromAccountId, cancellationToken);
                var toAccount = await _accountRepository.GetAsync(toAccountId, cancellationToken);

                amount = await _currencyConverter
                    .ConvertAsync(
                        amount - commission,
                        fromAccount.CurrencyCode,
                        toAccount.CurrencyCode,
                        cancellationToken);

                await AddMoneyAsync(toAccountId, amount, cancellationToken);

                _transferRepository.Create(
                    new Transfer
                    {
                        Money = amount,
                        CurrencyCode = toAccount.CurrencyCode,
                        FromAccountId = fromAccountId,
                        ToAccountId = toAccountId,
                    });

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            finally
            {
                await _unitOfWork.DisposeTransactionAsync();
            }
        }
    }
}
