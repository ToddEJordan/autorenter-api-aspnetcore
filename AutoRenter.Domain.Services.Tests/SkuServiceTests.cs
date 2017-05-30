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

        [Fact]
        public async void Get_ReturnsData()
        {
            // arrange
            var targetId = context.Skus.FirstOrDefault().Id;
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
            var result = await sut.Get(targetId);

            // assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async void Get_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var targetId = context.Skus.FirstOrDefault().Id;
            var targetEntity = await context.FindAsync<Sku>(targetId);

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

            var removeResult = context.Remove(targetEntity);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async void Insert_Succeeds()
        {
            // arrange
            var sku = context.Skus.First();

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Sku>()))
                .ReturnsAsync(() => true);

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

            context.Skus.Remove(sku);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(sku);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Insert_WhenDuplicateReturnsConflict()
        {
            // arrange
            var sku = context.Skus.First();

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Sku>()))
                .ReturnsAsync(() => true);

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
            var result = await sut.Insert(sku);

            // assert
            Assert.Equal(ResultCode.Conflict, result.ResultCode);
        }
        
        [Fact]
        public async void Insert_ReturnsId()
        {
            // arrange
            var sku = context.Skus.First();

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Sku>()))
                .ReturnsAsync(() => true);

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

            context.Skus.Remove(sku);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(sku);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Insert_InvalidReturnsBadRequest()
        {
            // arrange
            var sku = context.Skus.First();

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Sku>()))
                .ReturnsAsync(() => false);

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

            context.Skus.Remove(sku);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(sku);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Update_Succeeds()
        {
            // arrange
            var sku = context.Skus.First();
            sku.Color = "Orange";

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Sku>()))
                .ReturnsAsync(() => true);

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
            var result = await sut.Update(sku);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Update_ReturnsId()
        {
            // arrange
            var sku = context.Skus.First();
            sku.Color = "Orange";

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Sku>()))
                .ReturnsAsync(() => true);

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
            var result = await sut.Update(sku);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Update_InvalidReturnsBadRequest()
        {
            // arrange
            var sku = context.Skus.First();
            sku.Color = "Orange";

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Sku>()))
                .ReturnsAsync(() => false);

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
            var result = await sut.Insert(sku);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Delete_Succeeds()
        {
            // arrange
            var sku = context.Skus.First();

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Sku>()))
                .ReturnsAsync(() => true);

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
            var result = await sut.Delete(sku.Id);

            // assert
            Assert.Equal(ResultCode.Success, result);
        }

        [Fact]
        public async void Delete_NotFoundWhenNotFound()
        {
            // arrange
            var sku = context.Skus.First();

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Sku>()))
                .ReturnsAsync(() => true);

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

            context.Skus.Remove(sku);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Delete(sku.Id);

            // assert
            Assert.Equal(ResultCode.NotFound, result);
        }

        [Fact]
        public async void Delete_InvalidReturnsBadRequest()
        {
            // arrange
            var sku = context.Skus.First();

            ICommandFactory<Sku> commandFactory = new CommandFactory<Sku>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Sku>()))
                .ReturnsAsync(() => false);

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
            var result = await sut.Delete(sku.Id);

            // assert
            Assert.Equal(ResultCode.BadRequest, result);
        }
    }
}
