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

        [Fact]
        public async void Insert_Succeeds()
        {
            // arrange
            var model = context.Models.First();

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Model>()))
                .ReturnsAsync(() => true);

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            context.Models.Remove(model);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(model);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Insert_WhenDuplicateReturnsConflict()
        {
            // arrange
            var model = context.Models.First();

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Model>()))
                .ReturnsAsync(() => true);

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Insert(model);

            // assert
            Assert.Equal(ResultCode.Conflict, result.ResultCode);
        }
        
        [Fact]
        public async void Insert_ReturnsId()
        {
            // arrange
            var model = context.Models.First();

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Model>()))
                .ReturnsAsync(() => true);

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            context.Models.Remove(model);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(model);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Insert_InvalidReturnsBadRequest()
        {
            // arrange
            var model = context.Models.First();

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Model>()))
                .ReturnsAsync(() => false);

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);
            
            context.Models.Remove(model);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(model);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Update_Succeeds()
        {
            // arrange
            var model = context.Models.First();
            model.Name = "SomeName";

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Model>()))
                .ReturnsAsync(() => true);
            
            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Update(model);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Update_ReturnsId()
        {
            // arrange
            var model = context.Models.First();
            model.Name = "SomeName";

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Model>()))
                .ReturnsAsync(() => true);

            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Update(model);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Update_InvalidReturnsBadRequest()
        {
            // arrange
            var model = context.Models.First();
            model.Name = "SomeName";

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Model>()))
                .ReturnsAsync(() => false);
            
            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Insert(model);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Delete_Succeeds()
        {
            // arrange
            var model = context.Models.First();

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Model>()))
                .ReturnsAsync(() => true);
            
            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Delete(model.Id);

            // assert
            Assert.Equal(ResultCode.Success, result);
        }

        [Fact]
        public async void Delete_NotFoundWhenNotFound()
        {
            // arrange
            var model = context.Models.First();

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Model>()))
                .ReturnsAsync(() => true);
            
            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);
            
            context.Models.Remove(model);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Delete(model.Id);

            // assert
            Assert.Equal(ResultCode.NotFound, result);
        }

        [Fact]
        public async void Delete_InvalidReturnsBadRequest()
        {
            // arrange
            var model = context.Models.First();

            ICommandFactory<Model> commandFactory = new CommandFactory<Model>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Model>()))
                .ReturnsAsync(() => false);
            
            var sut = new ModelService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Delete(model.Id);

            // assert
            Assert.Equal(ResultCode.BadRequest, result);
        }
    }
}
