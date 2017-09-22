using System;
using Xunit;
using AutoRenter.Domain.Data;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services.Tests
{
    public class UserServiceTests : IDisposable
    {
        readonly AutoRenterContext _context;

        public UserServiceTests()
        {
            _context = TestableContextFactory.GenerateContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async void GetUserByUsernameAndPassword_ReturnsNotFoundWhenUsernameIsNotFound()
        {
            var userService = new UserService(_context);

            var result = await userService.GetUserByUsernameAndPassword("badusername", "wrong");

            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async void GetUserByUsernameAndPassword_ReturnsNotFoundWhenPasswordIsIncorrect()
        {
            var userService = new UserService(_context);

            var result = await userService.GetUserByUsernameAndPassword("janesmith", "wrong");

            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async void GetUserByUsernameAndPassword_ReturnsSuccess()
        {
            var userService = new UserService(_context);

            var result = await userService.GetUserByUsernameAndPassword("janesmith", "secret");

            Assert.Equal(ResultCode.Success, result.ResultCode);
            Assert.NotNull(result.Data);
        }
    }
}
