using Castle.Core.Logging;
using com.awawawiwa.Common.Constants;
using com.awawawiwa.Data.Context;
using com.awawawiwa.Data.Entities;
using com.awawawiwa.DTOs;
using com.awawawiwa.Security;
using com.awawawiwa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace com.awawawiwaTests.Services
{
    public class UserServiceTests
    {
        private UserService CreateService(UserContext context, Mock<IJwtService> jwtMock = null, Mock<IRevokedTokensService> revokedMock = null)
        {
            jwtMock ??= new Mock<IJwtService>();
            revokedMock ??= new Mock<IRevokedTokensService>();
            return new UserService(context, jwtMock.Object, revokedMock.Object, NullLogger<UserService>.Instance);
        }

        [Theory]
        [InlineData("username", "password", "email@email.com", true, null)]
        [InlineData("", "password", "email@email.com", false, "Username, Password and Email are required")]
        [InlineData("username", "", "email@email.com", false, "Username, Password and Email are required")]
        [InlineData("username", "password", "", false, "Username, Password and Email are required")]
        public async Task CreateUserAsyncTest(
            string username,
            string password,
            string email,
            bool expectedSuccess,
            string expectedMessage)
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new UserContext(options);
            var service = CreateService(context);

            var userInput = new CreateUserInputDTO
            {
                Username = username,
                Password = password,
                Email = email
            };

            var result = await service.CreateUserAsync(userInput);

            Assert.Equal(expectedSuccess, result.Success);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteUserAsync_UserExists_ShouldDelete()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new UserContext(options);
            var service = CreateService(context);

            var user = new UserEntity { UserId = Guid.NewGuid(), Username = "test", Password = "pass", Email = "test@test.com" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var result = await service.DeleteUserAsync(user.UserId);

            Assert.True(result.Success);
            Assert.Null(await context.Users.FindAsync(user.UserId));
        }

        [Fact]
        public async Task DeleteUserAsync_UserNotExists_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new UserContext(options);
            var service = CreateService(context);

            var result = await service.DeleteUserAsync(Guid.NewGuid());

            Assert.False(result.Success);
            Assert.Equal("UserNotFound", result.ErrorCode);
        }

        [Fact]
        public async Task LoginUserAsync_ValidCredentials_ShouldReturnToken()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new UserContext(options);
            var jwtMock = new Mock<IJwtService>();
            jwtMock.Setup(j => j.GenerateToken(It.IsAny<Guid>())).Returns("mockToken");

            var service = CreateService(context, jwtMock);

            var salt = PasswordHasherService.GenerateSalt();

            var hash = PasswordHasherService.ComputeHash("secret", salt, Constants.HASHING_ITERATIONS);

            var user = new UserEntity
            {
                UserId = Guid.NewGuid(),
                Username = "tester",
                Email = "t@test.com",
                Password = hash,
                Salt = salt
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var input = new LoginUserInputDTO { Username = "tester", Password = "secret" };

            var result = await service.LoginUserAsync(input);

            Assert.NotNull(result);
            Assert.Equal("mockToken", result.Token);
            Assert.Equal(user.UserId, result.UserId);
        }

        [Fact]
        public async Task LoginUserAsync_InvalidPassword_ShouldReturnNull()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new UserContext(options);
            var service = CreateService(context);

            var salt = PasswordHasherService.GenerateSalt();
            var hash = PasswordHasherService.ComputeHash("correct", salt, 255);

            context.Users.Add(new UserEntity
            {
                UserId = Guid.NewGuid(),
                Username = "tester",
                Password = hash,
                Salt = salt,
                Email = "e@e.com"
            });
            await context.SaveChangesAsync();

            var input = new LoginUserInputDTO { Username = "tester", Password = "wrong" };

            var result = await service.LoginUserAsync(input);

            Assert.Null(result);
        }

        [Fact]
        public async Task UploadProfilePictureAsync_InvalidFile_ShouldFail()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new UserContext(options);
            var service = CreateService(context);

            var user = new UserEntity { UserId = Guid.NewGuid(), Username = "fileuser", Password = "PW", Email = "f@f.com" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(0); // invalid

            var result = await service.UploadProfilePictureAsync(user.UserId, mockFile.Object);

            Assert.False(result.Success);
            Assert.Equal("InvalidProfilePicture", result.ErrorCode);
        }
    }
}
