using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using AutoRenter.Api.Models;
using AutoRenter.Api.Authorization;
using AutoRenter.Api.Tests.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using Moq;

namespace AutoRenter.Api.Tests
{
    public class TokenManagerTests
    {
        [Fact]
        public void CreateToken_ShouldReturnBearerString()
        {
            //arrange
            var appSettings = Options.Create(MockAppSettings().Object);
            var tokenManager = new TokenManager(appSettings, TestDateTime());
            
            //act
            var result = tokenManager.CreateToken(UserModelHelper.GetUser());

            //assert
            Assert.NotSame(-1, result.IndexOf("Bearer ", StringComparison.Ordinal));
        }

        [Fact]
        public void CreateToken_ShouldReturnResultOfExpectedLength()
        {
            //arrange
            var appSettings = Options.Create(MockAppSettings().Object);
            var tokenManager = new TokenManager(appSettings, TestDateTime());

            //act
            var result = tokenManager.CreateToken(UserModelHelper.GetUser());

            //assert
            Assert.NotSame(7, result.Length);
        }

        [Fact]
        public void CreateJsonWebToken_ShoulReturnJwtSecurityToken()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = MockAppSettings();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object), TestDateTime());

            //act
            var result = tokenManager.CreateJsonWebToken(userModel);

            //assert
            Assert.IsType(typeof(JwtSecurityToken), result);
        }

        [Fact]
        public void CreateJsonWebToken_TokenIssuerShouldMatchExpectedString()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = MockAppSettings();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object), TestDateTime());

            //act
            var result = tokenManager.CreateJsonWebToken(userModel);

            //assert
            Assert.Equal("FusionAlliance", result.Issuer);
        }

        [Fact]
        public void CreateJsonWebToken_TokenAudienceShouldMatchExpectedString()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = MockAppSettings();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object), TestDateTime());

            //act
            var result = tokenManager.CreateJsonWebToken(userModel);

            //assert
            Assert.Equal("ThoseWhoRock", result.Audiences.FirstOrDefault());
        }

        [Fact]
        public void CreateJsonWebToken_ResultShouldHaveCorrectClaimsCount()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = MockAppSettings();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object), TestDateTime());

            //act
            var result = tokenManager.CreateJsonWebToken(userModel);

            //assert
            Assert.Equal(9, result.Claims.Count());
        }

        [Fact]
        public void CreateJsonWebToken_ValidFromSultMatchTestDateTime()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = MockAppSettings();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object), TestDateTime());

            //act
            var result = tokenManager.CreateJsonWebToken(userModel);

            //assert
            Assert.Equal(TestDateTime(), result.ValidFrom);
        }

        [Fact]
        public void CreateJsonWebToken_ValidToShouldMatchExpectedDateTime()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = MockAppSettings();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object), TestDateTime());

            //act
            var result = tokenManager.CreateJsonWebToken(userModel);

            //assert
            Assert.Equal(TestDateTime().AddMinutes(5), result.ValidTo);
        }

        [Fact]
        public void CreateJsonWebToken_TokenIssuerShouldBeAccessedOnce()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = MockAppSettings();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object), TestDateTime());

            //act
            var result = tokenManager.CreateJsonWebToken(userModel);

            //assert
            appSettings.VerifyGet(i => i.TokenSettings.Issuer, Times.Once);
        }

        [Fact]
        public void CreateJsonWebToken_TokenAudienceShouldBeAccessedOnce()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = MockAppSettings();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object), TestDateTime());

            //act
            var result = tokenManager.CreateJsonWebToken(userModel);

            //assert
            appSettings.VerifyGet(i => i.TokenSettings.Audience, Times.Once);
        }

        [Fact]
        public void CreateJsonWebToken_TokenExperationMinutesShouldBeAccessedOnce()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = MockAppSettings();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object), TestDateTime());

            //act
            var result = tokenManager.CreateJsonWebToken(userModel);

            //assert
            appSettings.VerifyGet(i => i.TokenSettings.ExpirationMinutes, Times.Once);
        }

        [Fact]
        public void GetSigningCredentials_ShouldReturnSigningCredentials()
        {
            //arrange
            var appSettings = new Mock<AppSettings> {CallBase = true};
            appSettings.SetupGet(i => i.TokenSettings.Secret).Returns("ThisIsATest").Verifiable();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object));

            //act
            var result = tokenManager.GetSigningCredentials();

            //assert
            Assert.IsType(typeof(SigningCredentials), result);
        }
        
        [Fact]
        public void GetSigningCredentials_ShouldAccessTokenSettingsSecretOnce()
        {
            //arrange
            var appSettings = new Mock<AppSettings> {CallBase = true};
            appSettings.SetupGet(i => i.TokenSettings.Secret).Returns("ThisIsATest").Verifiable();
            var tokenManager = new TokenManager(Options.Create(appSettings.Object));

            //act
            var result = tokenManager.GetSigningCredentials();

            //assert
            appSettings.VerifyGet(i => i.TokenSettings.Secret, Times.Once);
        }

        [Fact]
        public void GetClaims_ShouldReturnArraryOfExpectedLength()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = Options.Create(new AppSettings());
            var tokenManager = new TokenManager(appSettings);

            //act
            var result = tokenManager.GetClaims(userModel);

            //assert
            Assert.Equal(5, result.Length);
        }

        [Fact]
        public void GetClaims_ShouldReturnMatchingUsernameClaim()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = Options.Create(new AppSettings());
            var tokenManager = new TokenManager(appSettings);

            //act
            var result = tokenManager.GetClaims(userModel);

            //assert
            Assert.Equal(userModel.Username, result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.Username).Value);
        }

        [Fact]
        public void GetClaims_ShouldReturnMatchingEmailCLaim()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = Options.Create(new AppSettings());
            var tokenManager = new TokenManager(appSettings);

            //act
            var result = tokenManager.GetClaims(userModel);

            //assert
            Assert.Equal(userModel.Email, result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.Email).Value);
        }

        [Fact]
        public void GetClaims_ShouldReturnMatchingFirstNameClaim()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = Options.Create(new AppSettings());
            var tokenManager = new TokenManager(appSettings);

            //act
            var result = tokenManager.GetClaims(userModel);

            //assert
            Assert.Equal(userModel.FirstName, result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.FirstName).Value);
        }

        [Fact]
        public void GetClaims_ShouldReturnMatchingLastNameClaim()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = Options.Create(new AppSettings());
            var tokenManager = new TokenManager(appSettings);

            //act
            var result = tokenManager.GetClaims(userModel);

            //assert
            Assert.Equal(userModel.LastName, result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.LastName).Value);
        }

        [Fact]
        public void GetClaims_ShouldReturnNonAdministratorUser()
        {
            //arrange
            var userModel = UserModelHelper.GetUser();
            var appSettings = Options.Create(new AppSettings());
            var tokenManager = new TokenManager(appSettings);

            //act
            var result = tokenManager.GetClaims(userModel);

            //assert
            Assert.False(Convert.ToBoolean(result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.IsAdministrator).Value));
        }

        [Fact]
        public void GetClaims_ShouldReturnListOfClaimsWithAdministoratorClaimWhenUserIsAdministrator()
        {
            //arrange
            var userModel = UserModelHelper.GetAdministratorUser();
            var appSettings = Options.Create(new AppSettings());
            var tokenManager = new TokenManager(appSettings);

            //act
            var result = tokenManager.GetClaims(userModel);

            //assert
            Assert.True(Convert.ToBoolean(result.ToList().FirstOrDefault(i => i.Type == AutoRenterClaimNames.IsAdministrator).Value));
        }

        private DateTime TestDateTime()
        {
            return new DateTime(2017, 1, 1).ToUniversalTime();
        }

        private Claim[] TestClaimsArray()
        {
            return new[]
            {
                new Claim("alg", "HS256"),
                new Claim("typ", "JWT"),
            };

        }

        private Mock<AppSettings> MockAppSettings()
        {
            var utcTime = TestDateTime();
            var appSettings = new Mock<AppSettings> { CallBase = true };
            appSettings.SetupGet(i => i.TokenSettings.Issuer).Returns("FusionAlliance").Verifiable();
            appSettings.SetupGet(i => i.TokenSettings.Audience).Returns("ThoseWhoRock").Verifiable();
            appSettings.SetupGet(i => i.TokenSettings.ExpirationMinutes).Returns(5).Verifiable();
            appSettings.SetupGet(i => i.TokenSettings.Secret).Returns("SecretSecretSecret").Verifiable();
            return appSettings;
        }
    }
}
