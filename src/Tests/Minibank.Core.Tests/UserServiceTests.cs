using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Domains.Users.Validators;
using Moq;
using Xunit;
using ValidationException = Minibank.Core.Exceptions.ValidationException;

namespace Minibank.Core.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IValidator<User> _validator;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _validator = new UserValidator();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _accountRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _validator);
        }

        [Fact]
        public async Task CreateUser_WithNullData_ShouldThrowException()
        {
            //ARRANGE

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() =>
                _userService.CreateAsync(null, CancellationToken.None));
        }

        [Fact]
        public async Task CreateUser_WithNullLogin_ShouldThrowException()
        {
            //ARRANGE
            var data = new User { Email = "1" };

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                _userService.CreateAsync(data, CancellationToken.None));
        }

        [Fact]
        public async Task CreateUser_WithNullEmail_ShouldThrowException()
        {
            //ARRANGE
            var data = new User { Login = "1" };

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                _userService.CreateAsync(data, CancellationToken.None));
        }

        [Fact]
        public async Task CreateUser_WithNullEmailAndLogin_ShouldThrowException()
        {
            //ARRANGE
            var data = new User();

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                _userService.CreateAsync(data, CancellationToken.None));
        }

        [Fact]
        public async Task CreateUser_WithInvalidData_ShouldNotCallCreate()
        {
            //ARRANGE
            var data = new User();

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _userService
                .CreateAsync(data, CancellationToken.None));

            _userRepositoryMock.Verify(repository => repository.Create(data), Times.Never);
        }

        [Fact]
        public async Task CreateUser_ErrorWithCreation_ShouldNotCallSaveChanges()
        {
            //ARRANGE
            var data = new User() {Email = "1", Login = "1"};

            _userRepositoryMock.Setup(repository => repository.Create(data)).Throws<Exception>();

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<Exception>(() => _userService
                .CreateAsync(data, CancellationToken.None));

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateUser_WithValidData_ShouldCallSaveChangesOnce()
        {
            //ARRANGE

            //ACT
            await _userService.CreateAsync(new User() {Email = "1", Login = "1"}, CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUser_WithValidId_ShouldReturnUserWithValidId()
        {
            //ARRANGE

            const int validId = 585;
            var validUser = new User { Id = validId };

            _userRepositoryMock
                .Setup(repository => repository
                    .GetAsync(validId, CancellationToken.None))
                .ReturnsAsync(validUser);

            //ACT
            var user = await _userService.GetAsync(validId, CancellationToken.None);

            //ASSERT
            Assert.Equal(validId, user.Id);
        }

        [Fact]
        public async Task GetUser_WithInvalidId_ShouldThrowException()
        {
            //ARRANGE

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => 
                _userService.GetAsync(1, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteUser_WithActiveAccounts_ShouldThrowException()
        {
            //ARRANGE
            _accountRepositoryMock.Setup(repository => repository
                    .IsActiveWithUserAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(true);

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() =>
                _userService.DeleteAsync(1, CancellationToken.None));
        }

        [Fact]
        public async Task DeleteUser_WithActiveAccounts_ShouldNotCallDeleteAsync()
        {
            //ARRANGE
            _accountRepositoryMock.Setup(repository => repository
                    .IsActiveWithUserAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(true);

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _userService
                .DeleteAsync(1, CancellationToken.None));

            _userRepositoryMock
                .Verify(repository => repository.DeleteAsync(1, CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_NotActiveAccountsOrNotExists_ShouldCallExistsAsync()
        {
            //ARRANGE

            //ACT
            await _userService.DeleteAsync(1, CancellationToken.None);

            //ASSERT
            _userRepositoryMock
                .Verify(repository => repository.ExistsAsync(1, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_UserNotExists_ShouldNotCallDeleteAsync()
        {
            //ARRANGE

            //ACT
            await _userService.DeleteAsync(1, CancellationToken.None);

            //ASSERT
            _userRepositoryMock
                .Verify(repository => repository.DeleteAsync(1, CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_ErrorWithDeleting_ShouldNotCallSaveChangesAsync()
        {
            //ARRANGE
            _userRepositoryMock.Setup(repository => repository
                    .ExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(true);

            _userRepositoryMock.Setup(repository => repository
                    .DeleteAsync(It.IsAny<int>(), CancellationToken.None))
                .Throws<Exception>();

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<Exception>(() => _userService
                .DeleteAsync(1, CancellationToken.None));

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_WithValidId_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            _userRepositoryMock.Setup(repository => repository
                    .ExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(true);

            //ACT
            await _userService.DeleteAsync(1, CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_UserNotExists_ShouldThrowException()
        {
            //ARRANGE

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() =>
                _userService.UpdateAsync(1, new User(), CancellationToken.None));
        }

        [Fact]
        public async Task UpdateUser_UserNotExists_ShouldNotCallUpdateAsync()
        {
            //ARRANGE
            var user = new User();

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<ValidationException>(() => _userService
                .UpdateAsync(1, user, CancellationToken.None));

            _userRepositoryMock.Verify(repository => repository
                .UpdateAsync(1, user, CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task UpdateUser_ErrorWithUpdating_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            _userRepositoryMock.Setup(repository => repository
                    .ExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(true);

            _userRepositoryMock.Setup(repository => repository
                    .UpdateAsync(It.IsAny<int>(), It.IsAny<User>(), CancellationToken.None))
                .Throws<Exception>();

            //ACT

            //ASSERT
            await Assert.ThrowsAsync<Exception>(() => _userService
                .UpdateAsync(1, null, CancellationToken.None));

            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateUser_WithValidId_ShouldCallSaveChangesOnce()
        {
            //ARRANGE
            _userRepositoryMock.Setup(repository => repository
                    .ExistsAsync(It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(true);

            //ACT
            await _userService.UpdateAsync(1, null, CancellationToken.None);

            //ASSERT
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
        }
    }
}
