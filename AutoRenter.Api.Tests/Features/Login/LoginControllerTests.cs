using System;
using AutoRenter.Api.Models;
using AutoRenter.Api.Controllers;
using AutoRenter.Api.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AutoRenter.Api.Tests.Features.Login
{
    [TestClass]

    public class LoginControllerTests
    {
        private Mock<IAuthenticateUser> _stubAuthenticateUser;

        [TestInitialize]
        public void TestInitialize()
        {
            _stubAuthenticateUser = new Mock<IAuthenticateUser>();
        }

        [TestMethod]
        public void Post_should_authenticate_user()
        {
            var mockAuthenticateUser = new Mock<IAuthenticateUser> { CallBase = true };
            mockAuthenticateUser.Setup(i => i.Execute(It.IsAny<LoginModel>())).Returns(new ResultModel());

            var mockLoginController = new Mock<LoginController>(mockAuthenticateUser.Object) { CallBase = true };

            var result = mockLoginController.Object.Post(new LoginModel()) as OkObjectResult;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var resultModel = (ResultModel) result.Value;
            Assert.IsTrue(resultModel.Success);

            mockAuthenticateUser.Verify(i => i.Execute(It.IsAny<LoginModel>()), Times.Once);
        }

        [TestMethod]
        public void Post_should_handle_error()
        {
            var errorMessage = "Something bad happened.";
            var mockAuthenticateUser = new Mock<IAuthenticateUser> { CallBase = true };
            mockAuthenticateUser.Setup(i => i.Execute(It.IsAny<LoginModel>())).Throws(new Exception(errorMessage));

            var mockLoginController = new Mock<LoginController>(mockAuthenticateUser.Object) { CallBase = true };

            var result = mockLoginController.Object.Post(new LoginModel()) as OkObjectResult;

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            mockAuthenticateUser.Verify(i => i.Execute(It.IsAny<LoginModel>()), Times.Once);
        }
    }
}
