using System;
using System.ComponentModel.DataAnnotations;
using AutoRenter.Api.Authentication;
using AutoRenter.Api.Controllers;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AutoRenter.Api.Tests
{
    public class LoginControllerTests
    {
        private Mock<IAuthenticateUser> authenticateUserMock;
        private Mock<IErrorCodeConverter> errorCodeConverterMock;

        public LoginControllerTests()
        {
            authenticateUserMock = new Mock<IAuthenticateUser>();
            errorCodeConverterMock = new Mock<IErrorCodeConverter>();
        }

        [Fact]
        public async void Post_ShouldReturnUnauthorizedResult()
        {
            authenticateUserMock.Setup(x => x.Execute(It.IsAny<LoginModel>()))
                .ReturnsAsync(() => new Result<UserModel>(ResultCode.Unauthorized));

            errorCodeConverterMock.Setup(x => x.Convert(It.IsAny<ResultCode>())).Returns(new UnauthorizedResult());

            var loginController = new LoginController(authenticateUserMock.Object, errorCodeConverterMock.Object);

            var result = await loginController.Post(new LoginModel());
            var unauthorizedResult = result as UnauthorizedResult;

            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async void Post_ShouldReturnBadRequest()
        {
            authenticateUserMock.Setup(x => x.Execute(It.IsAny<LoginModel>())).Throws(new Exception("An error occured."));

            var loginController = new LoginController(authenticateUserMock.Object, errorCodeConverterMock.Object);

            var result = await loginController.Post(new LoginModel());
            var badResult = result as BadRequestObjectResult;

            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal("An error occured.", badResult.Value);
        }

        [Fact]
        public async void Post_ShouldReturnOk()
        {
            authenticateUserMock.Setup(x => x.Execute(It.IsAny<LoginModel>()))
                .ReturnsAsync(() => new Result<UserModel>(ResultCode.Success, new UserModel { Username = "johndoe" }));

            var loginController = new LoginController(authenticateUserMock.Object, errorCodeConverterMock.Object);

            var result = await loginController.Post(new LoginModel());

            var okResult = result as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("johndoe", ((UserModel)okResult.Value).Username);
        }
    }
}
