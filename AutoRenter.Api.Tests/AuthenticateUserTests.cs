using Moq;
using Xunit;
using AutoRenter.Api.Authentication;
using AutoRenter.Api.Authorization;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests
{
    public class AuthenticateUserTests
    {
        [Fact]
        public void Execute_ShouldReturnNullDataInResultModel()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            var authenticateUser = new AuthenticateUser (tokenManager.Object);
            
            //act
            var result = authenticateUser.Execute(new LoginModel());

            //assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void Execute_ShouldReturnFailedLoginMessageInResultModel()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);
            
            //act
            var result = authenticateUser.Execute(new LoginModel());

            //assert
            Assert.Equal("Login failed.  Please try again.", result.Message);
        }

        [Fact]
        public void Execute_ShouldReturnUnsuccessfulResultModel()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);
            
            //act
            var result = authenticateUser.Execute(new LoginModel());

            //assert
            Assert.False(result.Success);
        }

        [Fact]
        public void Execute_NullAuthenticateUserShouldReturnNullDataInResult()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.Execute(new LoginModel());

            //assert
            Assert.Null(result.Data);
        }

        [Fact]
        public void Execute_NullAuthenticateUserShouldReturnUnsuccessfulResult()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);
            

            //act
            var result = authenticateUser.Execute(new LoginModel());

            //assert
            Assert.False(result.Success);
        }

        [Fact]
        public void Execute_NullAuthenticateUserShouldReturnErrorMessageInResult()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.Execute(new LoginModel());

            //assert
            Assert.Equal("Login failed.  Please try again.", result.Message);
        }

        [Fact]
        public void LookupUser_ShouldReturnUserModelWithExpectedBearerToken()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            tokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.LookupUser(new LoginModel { Username = "jdoe", Password = "user" });

            //assert
            Assert.Equal("token_token_token", result.BearerToken);
        }
                
        [Fact]
        public void LookupUser_ShouldAccessTokenManagerOnce()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            tokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.LookupUser(new LoginModel { Username = "jdoe", Password = "user" });

            //assert
            tokenManager.Verify(i => i.CreateToken(It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public void LookupUser_ShouldReturnNonAdministratorUserModel()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            tokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.LookupUser(new LoginModel { Username = "jdoe", Password = "user" });

            //assert
            Assert.False(result.IsAdministrator);
        }

        [Fact]
        public void LookupUser_ShouldReturnUserModelWithExpectedEmail()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            tokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.LookupUser(new LoginModel { Username = "jdoe", Password = "user" });

            //assert
            Assert.Equal("john.doe@fusionalliance.com", result.Email);
        }

                [Fact]
        public void LookupUser_ShouldReturnUserModelWithExpectedFirstName()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            tokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.LookupUser(new LoginModel { Username = "jdoe", Password = "user" });

            //assert
            Assert.Equal("John", result.FirstName);
        }


        [Fact]
        public void LookupUser_ShouldReturnUserModelWithExpectedLastName()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            tokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.LookupUser(new LoginModel { Username = "jdoe", Password = "user" });

            //assert
            Assert.Equal("Doe", result.LastName);
        }

        [Fact]
        public void LookupUser_ShouldReturnUserModelWithExpectedUsernamme()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            tokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.LookupUser(new LoginModel { Username = "jdoe", Password = "user" });

            //assert
            Assert.Equal("jdoe", result.Username);
        }


        [Fact]
        public void LookupUser_ShouldReturnAdministratorUserModel()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            tokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.LookupUser(new LoginModel { Username = "admin_jdoe", Password = "admin" });

            //assert
            Assert.True(result.IsAdministrator);
        }


        [Fact]
        public void LookupUser_ShouldReturnNull()
        {
            //arrange
            var tokenManager = new Mock<ITokenManager>();
            var authenticateUser = new AuthenticateUser(tokenManager.Object);

            //act
            var result = authenticateUser.LookupUser(new LoginModel { Password = "foo"});

            //assert
            Assert.Null(result);
        }
    }
}