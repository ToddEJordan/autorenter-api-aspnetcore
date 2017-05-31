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
