using AutoRenter.Api.Authentication;
using AutoRenter.Api.Authorization;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;
using Moq;
using Xunit;

namespace AutoRenter.Api.Tests.Authentication
{
    public class AuthenticateUserTests
    {
        private Mock<ITokenManager> tokenManagerMock;
        private Mock<IUserService> userServiceMock;
        private Mock<IDataStructureConverter> dataStructureConverterMock;

        public AuthenticateUserTests()
        {
            tokenManagerMock = new Mock<ITokenManager>();
            userServiceMock = new Mock<IUserService>();
            dataStructureConverterMock = new Mock<IDataStructureConverter>();
        }

        [Fact]
        public async void Execute_ShouldReturnUnauthorized()
        {
            userServiceMock.Setup(x => x.GetUserByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => new Result<User>(ResultCode.NotFound));

            var authenticateUser = new AuthenticateUser(tokenManagerMock.Object, userServiceMock.Object,
                dataStructureConverterMock.Object);

            var result = await authenticateUser.Execute(new LoginModel() {Username = "johndoe", Password = "bad"});

            Assert.Equal(ResultCode.Unauthorized, result.ResultCode);
        }

        [Fact]
        public async void Execute_ShouldReturnSuccess()
        {
            userServiceMock.Setup(x => x.GetUserByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => new Result<User>(ResultCode.Success, new User()));

            tokenManagerMock.Setup(x => x.CreateToken(It.IsAny<UserModel>())).Returns("tokentokentoken");

            dataStructureConverterMock.Setup(x => x.Map<UserModel, User>(It.IsAny<User>())).Returns(new UserModel());

            var authenticateUser = new AuthenticateUser(tokenManagerMock.Object, userServiceMock.Object,
                dataStructureConverterMock.Object);

            var result = await authenticateUser.Execute(new LoginModel() { Username = "johndoe", Password = "bad" });

            Assert.Equal(ResultCode.Success, result.ResultCode);
            Assert.Equal("tokentokentoken", result.Data.Token);
        }
    }
}