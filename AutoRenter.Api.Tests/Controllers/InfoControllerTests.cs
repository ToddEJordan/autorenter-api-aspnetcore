using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AutoRenter.Api.Controllers;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using AutoRenter.Api.Tests.Helpers;

namespace AutoRenter.Api.Tests.Controllers
{
    public class InfoControllerTests
    {
        [Fact]
        public async Task Get_ReturnsInfo()
        {
            // arrange
            var dataStructureConverterMoq = new Mock<IDataStructureConverter>();
            dataStructureConverterMoq.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(new Dictionary<string, object>
                        {
                            { "data", InfoHelper.Get() }
                        });

            var sut = new InfoController(dataStructureConverterMoq.Object);

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
            var dataStructureConverterMoq = new Mock<IDataStructureConverter>();
            dataStructureConverterMoq.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(new Dictionary<string, object>
                        {
                            { "data", InfoHelper.Get() }
                        });

            var sut = new InfoController(dataStructureConverterMoq.Object);

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
            var dataStructureConverterMoq = new Mock<IDataStructureConverter>();
            dataStructureConverterMoq.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(new Dictionary<string, object>
                        {
                            { "data", InfoHelper.Get() }
                        });

            var sut = new InfoController(dataStructureConverterMoq.Object);

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
            var dataStructureConverterMoq = new Mock<IDataStructureConverter>();
            dataStructureConverterMoq.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(new Dictionary<string, object>
                        {
                            { "data", InfoHelper.Get() }
                        });

            var sut = new InfoController(dataStructureConverterMoq.Object);

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
            var dataStructureConverterMoq = new Mock<IDataStructureConverter>();
            dataStructureConverterMoq.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(new Dictionary<string, object>
                        {
                            { "data", InfoHelper.Get() }
                        });

            var sut = new InfoController(dataStructureConverterMoq.Object);

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
