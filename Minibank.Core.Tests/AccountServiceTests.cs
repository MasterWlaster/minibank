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
using Moq;
using Xunit;

namespace Minibank.Core.Tests
{
    public class AccountServiceTests
    {
        [Fact]
        public void CalculateCommission_EqualAccounts_ShouldThrowException()
        {
            //ARRANGE
            var accountService = new AccountService(
                new Mock<IAccountRepository>().Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const int id = 540;

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .CalculateCommissionAsync(1, id, id, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void CalculateCommission_FromAccountNotExists_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const int fromAccountId = 234;
            const int toAccountId = 345;

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account()));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void CalculateCommission_ToAccountNotExists_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const int fromAccountId = 234;
            const int toAccountId = 345;

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account()));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void CalculateCommission_AccountsNotExists_ShouldThrowException()
        {
            //ARRANGE
            var accountService = new AccountService(
                new Mock<IAccountRepository>().Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .CalculateCommissionAsync(1, 1, 2, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void CalculateCommission_FromAccountNotActive_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const int fromAccountId = 234;
            const int toAccountId = 345;

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true }));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void CalculateCommission_ToAccountNotActive_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const int fromAccountId = 234;
            const int toAccountId = 345;

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true }));

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void CalculateCommission_AccountsNotActive_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const int fromAccountId = 234;
            const int toAccountId = 345;

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void CalculateCommission_EqualUserIds_ShouldReturnZero()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const int fromAccountId = 234;
            const int toAccountId = 345;
            const int userId = 111;

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, UserId = userId }));

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, UserId = userId }));

            //ACT
            var commission = accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            //ASSERT
            Assert.Equal(0, commission);
        }

        [Fact]
        public void CalculateCommission_NotEqualUserIds_ShouldReturnNotZero()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const int fromAccountId = 234;
            const int toAccountId = 345;
            const int fromUserId = 111;
            const int toUserId = 222;

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, UserId = fromUserId }));

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccountId, CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, UserId = toUserId }));

            //ACT
            var commission = accountService
                .CalculateCommissionAsync(1, fromAccountId, toAccountId, CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            //ASSERT
            Assert.True(commission != 0);
        }

        [Fact]
        public void CloseAccount_AccountNotExists_ShouldNotCallSaveChanges()
        {
            //ARRANGE
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountService = new AccountService(
                new Mock<IAccountRepository>().Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                unitOfWorkMock.Object);

            //ACT
            accountService.CloseAsync(1, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public void CloseAccount_AccountNotActive_ShouldNotCallSaveChanges()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                unitOfWorkMock.Object);

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() {IsActive = false}));

            //ACT
            accountService.CloseAsync(1, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public void CloseAccount_WithNotZeroBalance_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, Money = 1 }));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() =>
                accountService.CloseAsync(1, CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void CloseAccount_WithValidAccount_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                unitOfWorkMock.Object);

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() {IsActive = true, Money = 0}));

            //ACT
            accountService.CloseAsync(1, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void AddMoney_AccountNotExists_ShouldThrowException()
        {
            //ARRANGE
            var accountService = new AccountService(
                new Mock<IAccountRepository>().Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .AddMoneyAsync(1, 1, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void AddMoney_AccountNotActive_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = false }));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .AddMoneyAsync(1, 1, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void AddMoney_WithTooLowDelta_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const decimal delta = -100;
            const decimal money = 1;

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, Money = money }));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => accountService
                .AddMoneyAsync(1, delta, CancellationToken.None)
                .GetAwaiter()
                .GetResult());
        }

        [Fact]
        public void AddMoney_WithValidAccount_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                unitOfWorkMock.Object);

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(new Account() { IsActive = true, Money = 10 }));

            //ACT
            accountService.AddMoneyAsync(1, 1, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void CreateAccount_WithInvalidCurrency_ShouldThrowException()
        {
            //ARRANGE
            var accountService = new AccountService(
                new Mock<IAccountRepository>().Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            const string currencyCode = "invalidCode";

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() =>
                accountService.CreateAsync(1, currencyCode, CancellationToken.None)
                    .GetAwaiter()
                    .GetResult());
        }

        [Fact]
        public void CreateAccount_WithInvalidUserId_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                new Mock<IUnitOfWork>().Object);

            accountRepositoryMock.Setup(repository => repository
                    .Create(It.IsAny<int>(), It.IsAny<string>()))
                .Throws(new Exception());

            //ACT

            //ASSERT
            Assert.Throws<Exception>(() =>
                accountService.CreateAsync(1, "usd", CancellationToken.None)
                    .GetAwaiter()
                    .GetResult());
        }

        [Fact]
        public void CreateAccount_WithValidCurrencyAndUserId_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountService = new AccountService(
                new Mock<IAccountRepository>().Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                unitOfWorkMock.Object);

            //ACT
            accountService.CreateAsync(1, "usd", CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void DoTransfer_WithExceptionOnBeginTransaction_ShouldNotCallDisposeTransaction()
        {
            //ARRANGE
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountService = new AccountService(
                new Mock<IAccountRepository>().Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                unitOfWorkMock.Object);

            unitOfWorkMock
                .Setup(unitOfWork => unitOfWork.BeginTransactionAsync())
                .Throws(new Exception());

            //ACT
            try
            {
                accountService.DoTransferAsync(1, 1, 2, CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();
            }
            catch
            {
                //
            }

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.DisposeTransactionAsync(), Times.Never);
        }

        [Fact]
        public void DoTransfer_WithExceptionWhileDoingTransaction_ShouldCallDisposeTransactionOnce()
        {
            //ARRANGE
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountService = new AccountService(
                new Mock<IAccountRepository>().Object,
                new Mock<ITransferRepository>().Object,
                new Mock<ICurrencyConverter>().Object,
                unitOfWorkMock.Object);

            //ACT
            try
            {
                accountService.DoTransferAsync(1, 1, 2, CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();
            }
            catch
            {
                //
            }

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.DisposeTransactionAsync(), Times.Once);
        }

        [Fact]
        public void DoTransfer_Correct_ShouldCallDisposeTransactionOnce()
        {
            //ARRANGE
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var currencyConverterMock = new Mock<ICurrencyConverter>();
            var accountService = new AccountService(
                accountRepositoryMock.Object,
                new Mock<ITransferRepository>().Object,
                currencyConverterMock.Object,
                unitOfWorkMock.Object);

            const decimal fromAccountMoney = 1;
            var fromAccount = new Account() {Id = 1, Money = fromAccountMoney, IsActive = true, UserId = 1};
            var toAccount = new Account() {Id = 2, Money = 1, IsActive = true, UserId = 1};

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(fromAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(fromAccount));

            accountRepositoryMock.Setup(repository => repository
                    .GetAsync(toAccount.Id, CancellationToken.None))
                .Returns(Task.FromResult(toAccount));

            currencyConverterMock.Setup(converter => converter
                    .ConvertAsync(
                        fromAccountMoney, 
                        It.IsAny<string>(), 
                        It.IsAny<string>(), 
                        CancellationToken.None))
                .Returns(Task.FromResult(fromAccountMoney));

            //ACT
            try
            {
                accountService.DoTransferAsync(
                        fromAccountMoney, 
                        fromAccount.Id, 
                        toAccount.Id, 
                        CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();
            }
            catch
            {
                //
            }

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.DisposeTransactionAsync(), Times.Once);
        }
    }
}
