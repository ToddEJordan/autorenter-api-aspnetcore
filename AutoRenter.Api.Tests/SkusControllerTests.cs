using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AutoRenter.Api.Controllers;
using AutoRenter.Api.Services;
using AutoRenter.Api.Tests.Helpers;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests
{
    public class SkusControllerTests
    {
        [Fact]
        public async void GetAll_WhenFound()
        {
            // arrange
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.GetAll())
                .ReturnsAsync(() => new Result<IEnumerable<Sku>>(ResultCode.Success, TestSkus()));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.GetAll();
            var okResult = result as OkObjectResult;
            var response = okResult.Value as Dictionary<string, object>;

            // assert
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void GetAll_WhenNotFound()
        {
            // arrange
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.GetAll())
                .ReturnsAsync(() => new Result<IEnumerable<Sku>>(ResultCode.NotFound));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.GetAll();
            var notFoundResult = result as NotFoundResult;

            // assert
            Assert.NotNull(notFoundResult);
        }

        [Fact]
        public async void GetAll_ReturnsData()
        {
            // arrange
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.GetAll())
                .ReturnsAsync(() => new Result<IEnumerable<Sku>>(ResultCode.Success, TestSkus()));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.GetAll();
            var okResult = result as OkObjectResult;
            var response = okResult.Value as Dictionary<string, object>;

            // assert
            Assert.NotNull(response.Values);
        }

        [Fact]
        public async void Get_WhenFound()
        {
            // arrange
            var resultCodeProcessor = new ResultCodeProcessor();
            var targetId = new IdentifierHelper().SkuId;
            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<Sku>(ResultCode.Success, TestSku()));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Get(targetId);
            var okResult = result as OkObjectResult;
            var response = okResult.Value as Dictionary<string, object>;

            // assert
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async void Get_WhenNotFound()
        {
            // arrange
            var targetId = new IdentifierHelper().SkuId;
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<Sku>(ResultCode.NotFound));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Get(targetId);
            var notFoundResult = result as NotFoundResult;

            // assert
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async void Get_WithBadId()
        {
            // arrange
            var targetId = Guid.Empty;
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<Sku>(ResultCode.Success, TestSku()));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Get(targetId);
            var badRequestResult = result as BadRequestObjectResult;

            // assert
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async void Get_ReturnsData()
        {
            // arrange
            var targetId = new IdentifierHelper().SkuId;
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<Sku>(ResultCode.Success, TestSku()));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Get(targetId);
            var okResult = result as OkObjectResult;
            var response = okResult.Value as Dictionary<string, object>;

            // assert
            Assert.NotNull(response.Values);
        }

        [Fact]
        public async void Post_WhenValid()
        {
            // arrange
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Insert(It.IsAny<Sku>()))
                .ReturnsAsync(() => new Result<Guid>(ResultCode.Success, TestSku().Id));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Post(TestSku());
            var createdAtRouteResult = result as CreatedAtRouteResult;

            // assert
            Assert.NotNull(createdAtRouteResult);
        }

        [Fact]
        public async void Post_WhenNotValid()
        {
            // arrange
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Insert(It.IsAny<Sku>()))
                .ReturnsAsync(() => new Result<Guid>(ResultCode.BadRequest, TestSku().Id));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Post(TestSku());
            var badRequestResult = result as BadRequestResult;

            // assert
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async void Post_WhenConflict()
        {
            // arrange
            var testSku = TestSku();
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Insert(It.IsAny<Sku>()))
                .ReturnsAsync(() => new Result<Guid>(ResultCode.Conflict, testSku.Id));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Post(testSku);
            var conflictResult = result as StatusCodeResult;

            // assert
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async void Put_WhenValid()
        {
            // arrange
            var testSku = TestSku();
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Update(It.IsAny<Sku>()))
                .ReturnsAsync(() => new Result<Guid>(ResultCode.Success, testSku.Id));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Put(testSku.Id, testSku);
            var okResult = result as OkObjectResult;

            // assert
            Assert.NotNull(okResult);
        }

        [Fact]
        public async void Put_WhenNotValid()
        {
            // arrange
            var testSku = TestSku();
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Update(It.IsAny<Sku>()))
                .ReturnsAsync(() => new Result<Guid>(ResultCode.BadRequest, testSku.Id));

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Put(testSku.Id, testSku);
            var badRequestResult = result as BadRequestResult;

            // assert
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async void Delete_WhenValid()
        {
            // arrange
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Delete(It.IsAny<Guid>()))
                .ReturnsAsync(() => ResultCode.Success);

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Delete(TestSku().Id);
            var noContentResult = result as NoContentResult;

            // assert
            Assert.NotNull(noContentResult);
        }

        [Fact]
        public async void Delete_WhenNotValid()
        {
            // arrange
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Delete(It.IsAny<Guid>()))
                .ReturnsAsync(() => ResultCode.BadRequest);

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Delete(TestSku().Id);
            var badRequestResult = result as BadRequestResult;

            // assert
            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async void Delete_WhenNotFound()
        {
            // arrange
            var resultCodeProcessor = new ResultCodeProcessor();

            var skuServiceMoq = new Mock<ISkuService>();
            skuServiceMoq.Setup(x => x.Delete(It.IsAny<Guid>()))
                .ReturnsAsync(() => ResultCode.NotFound);

            var sut = new SkusController(skuServiceMoq.Object, resultCodeProcessor)
            {
                ControllerContext = DefaultControllerContext()
            };

            // act
            var result = await sut.Delete(TestSku().Id);
            var notFoundResult = result as NotFoundResult;

            // assert
            Assert.NotNull(notFoundResult);
        }

        private ControllerContext DefaultControllerContext()
        {
            return new ControllerContextHelper().GetContext();
        }

        private Sku TestSku()
        {
            return new SkuHelper().Get();
        }

        private IEnumerable<Sku> TestSkus()
        {
            return new SkuHelper().GetMany();
        }
    }
}
