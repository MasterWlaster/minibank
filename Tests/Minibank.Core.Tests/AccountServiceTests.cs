using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Accounts.Services;
using Minibank.Core.Domains.Transfers.Repositories;
using Minibank.Core.Exceptions;
using Minibank.Core.Exchanges;
using Minibank.Core.Helpers;
using Moq;
using Xunit;

namespace Minibank.Core.Tests
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ITransferRepository> _transferRepositoryMock;
        private readonly Mock<ICurrencyConverter> _currencyConverterMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICurrencyTool> _currencyToolMock;
        private readonly IAccountService _accountService;

        public AccountServiceTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _transferRepositoryMock = new Mock<ITransferRepository>();
            _currencyConverterMock = new Mock<ICurrencyConverter>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _currencyToolMock = new Mock<ICurrencyTool>();

            _accountService = new AccountService(
                _accountRepositoryMock.Object,
                _transferRepositoryMock.Object,
                _currencyConverterMock.Object,
                _unitOfWorkMock.Object,
                _currencyToolMock.Object);
        }

        [Fact]
        public async Task CalculateCommission_EqualAccounts_ShouldThrowException()
        {
            //ARRANGE
            const int id = 540;

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .CalculateCommissionAsync(1, id, id, CancellationToken.None));
        }

        [Fact]
        public async Task CalculateCommission_FromAccountNotExists_ShouldThrowException()
        {
            //ARRANGE
            const int fromAccountId = 234;
            const int toAccountId = 345;

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account()));

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None));
        }

        [Fact]
        public async Task CalculateCommission_ToAccountNotExists_ShouldThrowException()
        {
            //ARRANGE
            const int fromAccountId = 234;
            const int toAccountId = 345;

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account()));

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None));
        }

        [Fact]
        public async Task CalculateCommission_AccountsNotExists_ShouldThrowException()
        {
            //ARRANGE

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .CalculateCommissionAsync(1, 1, 2, CancellationToken.None));
        }

        [Fact]
        public async Task CalculateCommission_FromAccountNotActive_ShouldThrowException()
        {
            //ARRANGE
            const int fromAccountId = 234;
            const int toAccountId = 345;

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true }));

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None));
        }

        [Fact]
        public async Task CalculateCommission_ToAccountNotActive_ShouldThrowException()
        {
            //ARRANGE
            const int fromAccountId = 234;
            const int toAccountId = 345;

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true }));

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None));
        }

        [Fact]
        public async Task CalculateCommission_AccountsNotActive_ShouldThrowException()
        {
            //ARRANGE
            const int fromAccountId = 234;
            const int toAccountId = 345;

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None));
        }

        [Fact]
        public async Task CalculateCommission_EqualUserIds_ShouldReturnZero()
        {
            //ARRANGE
            const int fromAccountId = 234;
            const int toAccountId = 345;
            const int userId = 111;

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, UserId = userId }));

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, UserId = userId }));

            //ACT
            var commission = await _accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None);

            //ASSERT
            Assert.Equal(0, commission);
        }

        [Fact]
        public async Task CalculateCommission_NotEqualUserIds_ShouldReturnNotZero()
        {
            //ARRANGE
            const int fromAccountId = 234;
            const int toAccountId = 345;
            const int fromUserId = 111;
            const int toUserId = 222;

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, UserId = fromUserId }));

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, UserId = toUserId }));

            //ACT
            var commission = await _accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None);

            //ASSERT
            Assert.True(commission != 0);
        }

        [Fact]
        public async Task CloseAccount_InAnyCondition_ShouldCallGet()
        {
            //ARRANGE

            //ACT
            await _accountService.CloseAsync(1, CancellationToken.None);

            //ASSERT
            _accountRepositoryMock
                .Verify(repository => repository.GetAsync(1, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CloseAccount_AccountNotExists_ShouldNotCallSaveChanges()
        {
            //ARRANGE

            //ACT
            await _accountService.CloseAsync(1, CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CloseAccount_AccountNotActive_ShouldNotCallSaveChanges()
        {
            //ARRANGE
            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() {IsActive = false}));

            //ACT
            await _accountService.CloseAsync(1, CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CloseAccount_WithNotZeroBalance_ShouldThrowException()
        {
            //ARRANGE
            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, Money = 1 }));

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() =>
                _accountService.CloseAsync(1, CancellationToken.None));
        }

        [Fact]
        public async Task CloseAccount_WithValidAccount_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() {IsActive = true, Money = 0}));

            //ACT
            await _accountService.CloseAsync(1, CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddMoney_AccountNotExists_ShouldThrowException()
        {
            //ARRANGE

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .AddMoneyAsync(1, 1, CancellationToken.None));
        }

        [Fact]
        public async Task AddMoney_AccountNotActive_ShouldThrowException()
        {
            //ARRANGE
            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .AddMoneyAsync(1, 1, CancellationToken.None));
        }

        [Fact]
        public async Task AddMoney_WithTooLowDelta_ShouldThrowException()
        {
            //ARRANGE
            const decimal delta = -100;
            const decimal money = 1;

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, Money = money }));

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _accountService
                .AddMoneyAsync(1, delta, CancellationToken.None));
        }

        [Fact]
        public async Task AddMoney_WithValidAccount_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, Money = 10 }));

            //ACT
            await _accountService.AddMoneyAsync(1, 1, CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAccount_WithInvalidCurrency_ShouldThrowException()
        {
            //ARRANGE

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() =>
                _accountService.CreateAsync(1, "", CancellationToken.None));
        }

        [Fact]
        public async Task CreateAccount_WithInvalidUserId_ShouldThrowException()
        {
            //ARRANGE
            const string validCurrency = "currency";

            _accountRepositoryMock.Setup(repository => repository
                    .Create(It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception());

            _currencyToolMock.Setup(tool => tool.Validate(It.IsAny<string>()))
                .Returns(validCurrency);

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<Exception>(() =>
                _accountService.CreateAsync(1, "", CancellationToken.None));
        }

        [Fact]
        public async Task CreateAccount_WithValidCurrencyAndUserId_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            const string validCurrency = "currency";

            _currencyToolMock.Setup(tool => tool.Validate(It.IsAny<string>()))
                .Returns(validCurrency);

            //ACT
            await _accountService.CreateAsync(1, "", CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DoTransfer_WithExceptionOnBeginTransaction_ShouldNotCallDisposeTransaction()
        {
            //ARRANGE
            _unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.BeginTransactionAsync())
                .Throws(new Exception());

            //ACT
            try
            {
                await _accountService
                    .DoTransferAsync(1, 1, 2, CancellationToken.None);
            }
            catch
            {
                //
            }

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.DisposeTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task DoTransfer_WithExceptionWhileDoingTransaction_ShouldCallDisposeTransactionOnce()
        {
            //ARRANGE

            //ACT
            try
            {
                await _accountService
                    .DoTransferAsync(1, 1, 2, CancellationToken.None);
            }
            catch
            {
                //
            }

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.DisposeTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task DoTransfer_Correct_ShouldCallDisposeTransactionOnce()
        {
            //ARRANGE
            const decimal fromAccountMoney = 1;
            var fromAccount = new Account() {Id = 1, Money = fromAccountMoney, IsActive = true, UserId = 1};
            var toAccount = new Account() {Id = 2, Money = 1, IsActive = true, UserId = 1};

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(fromAccount));

            _accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(toAccount));

            _currencyConverterMock.Setup(converter => converter
                    .ConvertAsync(
                        fromAccountMoney, 
                        It.IsAny<string>(), 
                        It.IsAny<string>(), 
                        CancellationToken.None))
                .Returns(Task.FromResult(fromAccountMoney));

            //ACT
            try
            {
                await _accountService.DoTransferAsync(
                    fromAccountMoney, 
                    fromAccount.Id, 
                    toAccount.Id, 
                    CancellationToken.None);
            }
            catch
            {
                //
            }

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.DisposeTransactionAsync(), Times.Once);
        }
    }
}
