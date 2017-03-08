using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using AutoRenter.Api.Authorization;
using AutoRenter.Api.Features.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AutoRenter.Api.Tests.Authorization
{
    [TestClass]
    public class TokenManagerTests
    {
        private IOptions<AppSettings> _stubAppSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _stubAppSettings = Options.Create(new AppSettings());
        }

        [TestMethod]
        public void CreateToken_should_return_string()
        {
            var mockTokenManager = new Mock<TokenManager>(_stubAppSettings) { CallBase = true };
            mockTokenManager.Setup(i => i.CreateJsonWebToken(It.IsAny<UserModel>())).Returns(new JwtSecurityToken()).Verifiable();

            var result = mockTokenManager.Object.CreateToken(new UserModel());

            Assert.AreNotSame(-1, result.IndexOf("Bearer ", StringComparison.Ordinal));
            Assert.AreNotSame(7, result.Length);
            mockTokenManager.Verify(i => i.CreateJsonWebToken(It.IsAny<UserModel>()), Times.Once);
        }

        [TestMethod]
        public void CreateJsonWebToken_should_return_JwtSecurityToken()
        {
            var utcTime = new DateTime(2017, 1, 1).ToUniversalTime();

            var userModel = new UserModel
            {
                Username = "jdoe",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe",
                IsAdministrator = false
            };

            var claims = new[]
            {
                new Claim("alg", "HS256"),
                new Claim("typ", "JWT"),
            };

            var mockAppSettings = new Mock<AppSettings> { CallBase = true };
            mockAppSettings.SetupGet(i => i.TokenSettings.Issuer).Returns("FusionAlliance").Verifiable();
            mockAppSettings.SetupGet(i => i.TokenSettings.Audience).Returns("ThoseWhoRock").Verifiable();
            mockAppSettings.SetupGet(i => i.TokenSettings.ExpirationMinutes).Returns(5).Verifiable();
            mockAppSettings.SetupGet(i => i.TokenSettings.Secret).Returns("SecretSecretSecret").Verifiable();

            var mockTokenManager = new Mock<TokenManager>(Options.Create(mockAppSettings.Object), utcTime) { CallBase = true };
            mockTokenManager.Setup(i => i.GetClaims(It.IsAny<UserModel>())).Returns(claims).Verifiable();
            mockTokenManager.Setup(i => i.GetSigningCredentials()).Returns(It.IsAny<SigningCredentials>).Verifiable();

            var result = mockTokenManager.Object.CreateJsonWebToken(userModel);

            Assert.IsInstanceOfType(result, typeof(JwtSecurityToken));
            Assert.AreEqual("FusionAlliance", result.Issuer);
            Assert.AreEqual("ThoseWhoRock", result.Audiences.FirstOrDefault());
            Assert.AreEqual(6, result.Claims.Count());
            Assert.AreEqual(utcTime, result.ValidFrom);
            Assert.AreEqual(utcTime.AddMinutes(5), result.ValidTo);

            mockAppSettings.VerifyGet(i => i.TokenSettings.Issuer, Times.Once);
            mockAppSettings.VerifyGet(i => i.TokenSettings.Audience, Times.Once);
            mockAppSettings.VerifyGet(i => i.TokenSettings.ExpirationMinutes, Times.Once);
            mockTokenManager.Verify(i => i.GetClaims(It.IsAny<UserModel>()), Times.Once);
            mockTokenManager.Verify(i => i.GetSigningCredentials(), Times.Once);
        }

        [TestMethod]
        public void GetSigningCredentials_should_return_SigningCredentials()
        {
            var mockAppSettings = new Mock<AppSettings> {CallBase = true};
            mockAppSettings.SetupGet(i => i.TokenSettings.Secret).Returns("ThisIsATest").Verifiable();

            var mockTokenManager = new Mock<TokenManager>(Options.Create(mockAppSettings.Object)) { CallBase = true };

            var result = mockTokenManager.Object.GetSigningCredentials();

            Assert.IsInstanceOfType(result, typeof(SigningCredentials));
            mockAppSettings.VerifyGet(i => i.TokenSettings.Secret, Times.Once);
        }

        [TestMethod]
        public void should_return_array_of_claims_from_GetClaims()
        {
            var userModel = new UserModel
            {
                Username = "jdoe",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe",
                IsAdministrator = false
            };

            var mockTokenManager = new Mock<TokenManager>(_stubAppSettings) { CallBase = true };

            var result = mockTokenManager.Object.GetClaims(userModel);

            Assert.AreEqual(5, result.Length);
            Assert.AreEqual(userModel.Username, result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.Username).Value);
            Assert.AreEqual(userModel.Email, result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.Email).Value);
            Assert.AreEqual(userModel.FirstName, result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.FirstName).Value);
            Assert.AreEqual(userModel.LastName, result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.LastName).Value);
            Assert.IsFalse(Convert.ToBoolean(result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.IsAdministrator).Value));
        }

        [TestMethod]
        public void should_return_array_of_claims_from_GetClaims_when_user_is_administrator()
        {
            var userModel = new UserModel
            {
                Username = "admin_jdoe",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe",
                IsAdministrator = true
            };

            var mockTokenManager = new Mock<TokenManager>(_stubAppSettings) { CallBase = true };

            var result = mockTokenManager.Object.GetClaims(userModel);

            Assert.IsTrue(Convert.ToBoolean(result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.IsAdministrator).Value));
        }
    }
}
