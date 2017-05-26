using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using AutoRenter.Api.Controllers;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests
{
    public class InfoControllerTests
    {
        [Fact]
        public async Task Get_ReturnsInfo()
        {
            // arrange
            var sut = new InfoController();

            // act
            var response = await sut.Get();
            var result = response as OkObjectResult;

            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_ReturnsTitle()
        {
            // arrange
            var sut = new InfoController();

            // act
            var response = await sut.Get();
            var result = response as OkObjectResult;
            var resultValue = (Dictionary<string, object>)result.Value;

            var infoResult = (ApiInfoModel)resultValue["data"];

            // assert
            Assert.True(infoResult.Title.Length > 0);
        }

        [Fact]
        public async Task Get_ReturnsEnvironment()
        {
            // arrange
            var sut = new InfoController();

            // act
            var response = await sut.Get();
            var result = response as OkObjectResult;
            var resultValue = (Dictionary<string, object>)result.Value;

            var infoResult = (ApiInfoModel)resultValue["data"];

            // assert
            Assert.True(infoResult.Environment.Length > 0);
        }

        [Fact]
        public async Task Get_ReturnsVersion()
        {
            // arrange
            var sut = new InfoController();

            // act
            var response = await sut.Get();
            var result = response as OkObjectResult;
            var resultValue = (Dictionary<string, object>)result.Value;

            var infoResult = (ApiInfoModel)resultValue["data"];

            // assert
            Assert.True(infoResult.Version.Length > 0);
        }

        [Fact]
        public async Task Get_ReturnsBuild()
        {
            // arrange
            var sut = new InfoController();

            // act
            var response = await sut.Get();
            var result = response as OkObjectResult;
            var resultValue = (Dictionary<string, object>)result.Value;

            var infoResult = (ApiInfoModel)resultValue["data"];

            // assert
            Assert.True(infoResult.Build.Length > 0);
        }
    }
}
