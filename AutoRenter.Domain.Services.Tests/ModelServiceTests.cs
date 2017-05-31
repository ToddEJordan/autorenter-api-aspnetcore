using System;
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
    public class ModelServiceTests : IDisposable
    {
        AutoRenterContext context;

        public ModelServiceTests()
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
            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);
            
            // act
            var result = await sut.GetAll();

            // assert
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async void GetAll_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var allModel = await context.Set<Model>().ToListAsync();

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            context.Models.RemoveRange(allModel);
            await context.SaveChangesAsync();

            // act
            var result = await sut.GetAll();

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async void Get_ReturnsData()
        {
            // arrange
            var targetId = context.Models.FirstOrDefault().Id;
            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async void Get_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var targetId = context.Models.FirstOrDefault().Id;
            var targetEntity = await context.FindAsync<Model>(targetId);

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            var removeResult = context.Remove(targetEntity);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async void GetByExternalId_ReturnsData()
        {
            // arrange
            var targetId = context.Models.FirstOrDefault().ExternalId;
            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async void GetByExternalId_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var targetId = context.Models.FirstOrDefault().Id;
            var targetEntity = await context.FindAsync<Model>(targetId);
            var targetExternalId = targetEntity.ExternalId;

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            var removeResult = context.Remove(targetEntity);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Get(targetExternalId);

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }
    }
}
