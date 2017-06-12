using AutoRenter.Api.Authorization;
using AutoRenter.Api.Models;
using AutoRenter.Api.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AutoRenter.Api.OldTests.Features.Login
{
    [TestClass]
    [Ignore]
    public class AuthenticateUserTests
    {
        private Mock<ITokenManager> _stubTokenManager;

        [TestInitialize]
        public void TestInitialize()
        {
            _stubTokenManager = new Mock<ITokenManager>();
        }

        [TestMethod]
        public void Execute_should_return_ResultModel()
        {
            var mockAuthenticateUser = new Mock<AuthenticateUser>(_stubTokenManager.Object) { CallBase = true };
            mockAuthenticateUser.Setup(i => i.LookupUser(It.IsAny<LoginModel>())).Returns(new UserModel());

            var result = mockAuthenticateUser.Object.Execute(new LoginModel());

            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(null, result.Message);
        }

        [TestMethod]
        public void Execute_should_return_ResultModel_with_error()
        {
            var mockAuthenticateUser = new Mock<AuthenticateUser>(_stubTokenManager.Object) { CallBase = true };
            mockAuthenticateUser.Setup(i => i.LookupUser(It.IsAny<LoginModel>())).Returns((UserModel)null);

            var result = mockAuthenticateUser.Object.Execute(new LoginModel());

            Assert.IsNull(result.Data);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Login failed.  Please try again.", result.Message);
        }

        [TestMethod]
        public void LookupUser_should_return_UserModel()
        {
            var mockTokenManager = new Mock<ITokenManager> {CallBase = true};
            mockTokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();

            var mockAuthenticateUser = new Mock<AuthenticateUser>(mockTokenManager.Object) { CallBase = true };

            var result = mockAuthenticateUser.Object.LookupUser(new LoginModel { Username = "jdoe", Password = "secret" });

            Assert.AreEqual("john.doe@fusionalliance.com", result.Email);
            Assert.IsFalse(result.IsAdministrator);
            Assert.AreEqual("token_token_token", result.BearerToken);

            mockTokenManager.Verify(i => i.CreateToken(It.IsAny<UserModel>()), Times.Once);
        }

        [TestMethod]
        public void LookupUser_should_return_UserModel_with_IsAdministrator_true()
        {
            var mockTokenManager = new Mock<ITokenManager> { CallBase = true };
            mockTokenManager.Setup(i => i.CreateToken(It.IsAny<UserModel>())).Returns("token_token_token").Verifiable();

            var mockAuthenticateUser = new Mock<AuthenticateUser>(mockTokenManager.Object) { CallBase = true };

            var result = mockAuthenticateUser.Object.LookupUser(new LoginModel { Username = "admin_jdoe", Password = "secret" });

            Assert.IsTrue(result.IsAdministrator);

            mockTokenManager.Verify(i => i.CreateToken(It.IsAny<UserModel>()), Times.Once);
        }


        [TestMethod]
        public void LookupUser_should_return_null()
        {
            var mockAuthenticateUser = new Mock<AuthenticateUser>(_stubTokenManager.Object) { CallBase = true };

            var result = mockAuthenticateUser.Object.LookupUser(new LoginModel { Password = "foo"});

            Assert.IsNull(result);
        }
    }
}
