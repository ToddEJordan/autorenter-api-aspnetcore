using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Services.Commands;

namespace AutoRenter.Domain.Services.Tests
{
    public class SkuServiceTests : IDisposable
    {
        AutoRenterContext context;

        public SkuServiceTests()
        {
            // xUnit.net creates a new instance of the test class for every test that is run.
            // The constructor and the Dispose method will always be called once for every test.
            context = TestableContextFactory.GenerateContext();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public async void GetAll_ReturnsData()
        {
            // arrange
            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.FirstOrDefault()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.FirstOrDefault()));

            var sut = new SkuService(context, 
                commandFactory, 
                validationServiceMoq.Object, 
                makeServiceMoq.Object, 
                modelServiceMoq.Object);
            
            // act
            var result = await sut.GetAll();

            // assert
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async void GetAll_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var allSku = await context.Set<Sku>().ToListAsync();

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.FirstOrDefault()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.FirstOrDefault()));

            var sut = new SkuService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            context.Skus.RemoveRange(allSku);
            await context.SaveChangesAsync();

            // act
            var result = await sut.GetAll();

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }
    }
}
