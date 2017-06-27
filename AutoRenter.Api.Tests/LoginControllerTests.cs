using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AutoRenter.Api.Authentication;
using AutoRenter.Api.Controllers;
using AutoRenter.Api.Models;

namespace AutoRenter.Api
{
    public class LoginControllerTests
    {
        [Fact]
        public void Post_AccessesAuthencateUserObjectOnce(){
            //arrange
            var authenticateUser = new Mock<IAuthenticateUser>();
            authenticateUser.Setup(i => 
                    i.Execute(It.IsAny<LoginModel>()))
                .Returns(new ResultModel());
            var loginController = new LoginController(authenticateUser.Object);

            //act
            loginController.Post( new LoginModel());
            
            //assert
            authenticateUser.Verify(i => 
                    i.Execute(It.IsAny<LoginModel>()), 
                Times.Once);            

        }

        [Fact]
        public void Post_ValidUserServiceReturnsSucessfulResult()
        {
            //arrange
            var authenticateUser = new Mock<IAuthenticateUser>();
            authenticateUser.Setup(i => 
                    i.Execute(It.IsAny<LoginModel>()))
                .Returns(new ResultModel());
            var loginController = new LoginController(authenticateUser.Object);

            //act
            var result = loginController.Post( new LoginModel()) 
                as OkObjectResult;
            var resultModel = (ResultModel) result.Value;

            //assert            
            Assert.True(resultModel.Success);
        }

        [Fact]
        public void Post_InvalidUserService()
        {
            //arrange
            var errorMessage = "Something bad happened.";
            var authenticateUser = new Mock<IAuthenticateUser>();
            authenticateUser.Setup(i => i.Execute(It.IsAny<LoginModel>())).Throws(new Exception(errorMessage));
            var loginController = new LoginController(authenticateUser.Object);

            //act
            var result = loginController.Post(new LoginModel()) as BadRequestObjectResult;            

            //assert
            Assert.IsType(typeof(BadRequestObjectResult), result);
        }       
    }   
}
