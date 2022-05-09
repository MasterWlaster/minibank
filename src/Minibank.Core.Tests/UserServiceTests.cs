using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Domains.Users.Validators;
using Minibank.Data;
using Minibank.Data.Accounts.Repositories;
using Minibank.Data.Users.Repositories;
using Moq;
using Xunit;
using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void CreateUser_WithNullLogin_ShouldThrowException()
        {
            //ARRANGE
            var userValidator = new UserValidator();
            var userService = new UserService(
                new Mock<IUserRepository>().Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IUnitOfWork>().Object,
                userValidator);

            var data = new User { Email = "1" };

            //ACT

            //ASSERT
            Assert.Throws<FluentValidation.ValidationException>(() =>
                userService.CreateAsync(data, CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void CreateUser_WithNullEmail_ShouldThrowException()
        {
            //ARRANGE
            var userValidator = new UserValidator();
            var userService = new UserService(
                new Mock<IUserRepository>().Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IUnitOfWork>().Object,
                userValidator);

            var data = new User { Login = "1" };

            //ACT

            //ASSERT
            Assert.Throws<FluentValidation.ValidationException>(() =>
                userService.CreateAsync(data, CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void CreateUser_WithNullEmailAndLogin_ShouldThrowException()
        {
            //ARRANGE
            var userValidator = new UserValidator();
            var userService = new UserService(
                new Mock<IUserRepository>().Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IUnitOfWork>().Object,
                userValidator);

            var data = new User();

            //ACT

            //ASSERT
            Assert.Throws<FluentValidation.ValidationException>(() =>
                userService.CreateAsync(data, CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void CreateUser_WithNullData_ShouldThrowException()
        {
            //ARRANGE
            var userValidator = new UserValidator();
            var userService = new UserService(
                new Mock<IUserRepository>().Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IUnitOfWork>().Object,
                userValidator);

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() =>
                userService.CreateAsync(null, CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void CreateUser_WithValidData_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userService = new UserService(
                new Mock<IUserRepository>().Object,
                new Mock<IAccountRepository>().Object,
                unitOfWorkMock.Object,
                new Mock<IValidator<User>>().Object);

            //ACT
            userService.CreateAsync(new User(), CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void GetUser_WithValidId_ShouldReturnUserWithValidId()
        {
            //ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(
                userRepositoryMock.Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IUnitOfWork>().Object,
                new Mock<IValidator<User>>().Object);

            const int validId = 585;
            var validUser = new User { Id = validId };

            userRepositoryMock
                .Setup(repository => repository
                    .GetAsync(validId, CancellationToken.None))
                .Returns(Task.FromResult(validUser));

            //ACT
            var user = userService.GetAsync(validId, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(validId, user.Id);
        }

        [Fact]
        public void GetUser_WithInvalidId_ShouldThrowException()
        {
            //ARRANGE
            var userService = new UserService(
                new Mock<IUserRepository>().Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IUnitOfWork>().Object,
                new Mock<IValidator<User>>().Object);

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() => 
                userService.GetAsync(1, CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void DeleteUser_UserNotExists_ShouldNotCallSaveChangesAsync()
        {
            //ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userService = new UserService(
                userRepositoryMock.Object,
                new Mock<IAccountRepository>().Object,
                unitOfWorkMock.Object,
                new Mock<IValidator<User>>().Object);

            userRepositoryMock.Setup(repository => repository
                .ExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(false));

            //ACT
            userService.DeleteAsync(1, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public void DeleteUser_WithActiveAccounts_ShouldThrowException()
        {
            //ARRANGE
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var userService = new UserService(
                new Mock<IUserRepository>().Object,
                accountRepositoryMock.Object,
                new Mock<IUnitOfWork>().Object,
                new Mock<IValidator<User>>().Object);

            accountRepositoryMock.Setup(repository => repository
                    .IsActiveWithUserAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(true));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() =>
                userService.DeleteAsync(1, CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void DeleteUser_WithValidId_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(
                userRepositoryMock.Object,
                new Mock<IAccountRepository>().Object,
                unitOfWorkMock.Object,
                new Mock<IValidator<User>>().Object);

            userRepositoryMock.Setup(repository => repository
                    .ExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(true));

            //ACT
            userService.DeleteAsync(1, CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void UpdateUser_UserNotExists_ShouldThrowException()
        {
            //ARRANGE
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(
                userRepositoryMock.Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IUnitOfWork>().Object,
                new Mock<IValidator<User>>().Object);

            userRepositoryMock.Setup(repository => repository
                    .ExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(false));

            //ACT

            //ASSERT
            Assert.Throws<ValidationException>(() =>
                userService.UpdateAsync(1, new User(), CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void UpdateUser_WithValidId_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(
                userRepositoryMock.Object,
                new Mock<IAccountRepository>().Object,
                unitOfWorkMock.Object,
                new Mock<IValidator<User>>().Object);

            userRepositoryMock.Setup(repository => repository
                    .ExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .Returns(Task.FromResult(true));

            //ACT
            userService.UpdateAsync(1, null, CancellationToken.None)
                .GetAwaiter()
                .GetResult();

            //ASSERT
            unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }
    }
}
