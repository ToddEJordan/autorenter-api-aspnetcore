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
    public class MakeServiceTests : IDisposable
    {
        AutoRenterContext context;

        public MakeServiceTests()
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
            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);
            
            // act
            var result = await sut.GetAll();

            // assert
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async void GetAll_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var allMake = await context.Set<Make>().ToListAsync();

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            context.Makes.RemoveRange(allMake);
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
            var targetId = context.Makes.FirstOrDefault().Id;
            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async void Get_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var targetId = context.Makes.FirstOrDefault().Id;
            var targetEntity = await context.FindAsync<Make>(targetId);

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

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
            var targetId = context.Makes.FirstOrDefault().ExternalId;
            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async void GetByExternalId_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var targetId = context.Makes.FirstOrDefault().Id;
            var targetEntity = await context.FindAsync<Make>(targetId);
            var targetExternalId = targetEntity.ExternalId;

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();
            var validationServiceMoq = new Mock<IValidationService>();

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

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
            var make = context.Makes.First();

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Make>()))
                .ReturnsAsync(() => true);

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            context.Makes.Remove(make);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(make);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Insert_WhenDuplicateReturnsConflict()
        {
            // arrange
            var make = context.Makes.First();

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Make>()))
                .ReturnsAsync(() => true);

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Insert(make);

            // assert
            Assert.Equal(ResultCode.Conflict, result.ResultCode);
        }
        
        [Fact]
        public async void Insert_ReturnsId()
        {
            // arrange
            var make = context.Makes.First();

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Make>()))
                .ReturnsAsync(() => true);

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            context.Makes.Remove(make);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(make);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Insert_InvalidReturnsBadRequest()
        {
            // arrange
            var make = context.Makes.First();

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Make>()))
                .ReturnsAsync(() => false);

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);
            
            context.Makes.Remove(make);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(make);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Update_Succeeds()
        {
            // arrange
            var make = context.Makes.First();
            make.Name = "SomeName";

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Make>()))
                .ReturnsAsync(() => true);
            
            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Update(make);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Update_ReturnsId()
        {
            // arrange
            var make = context.Makes.First();
            make.Name = "SomeName";

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Make>()))
                .ReturnsAsync(() => true);

            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Update(make);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Update_InvalidReturnsBadRequest()
        {
            // arrange
            var make = context.Makes.First();
            make.Name = "SomeName";

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Make>()))
                .ReturnsAsync(() => false);
            
            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Insert(make);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Delete_Succeeds()
        {
            // arrange
            var make = context.Makes.First();

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Make>()))
                .ReturnsAsync(() => true);
            
            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Delete(make.Id);

            // assert
            Assert.Equal(ResultCode.Success, result);
        }

        [Fact]
        public async void Delete_NotFoundWhenNotFound()
        {
            // arrange
            var make = context.Makes.First();

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Make>()))
                .ReturnsAsync(() => true);
            
            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);
            
            context.Makes.Remove(make);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Delete(make.Id);

            // assert
            Assert.Equal(ResultCode.NotFound, result);
        }

        [Fact]
        public async void Delete_InvalidReturnsBadRequest()
        {
            // arrange
            var make = context.Makes.First();

            ICommandFactory<Make> commandFactory = new CommandFactory<Make>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Make>()))
                .ReturnsAsync(() => false);
            
            var sut = new MakeService(context, commandFactory, validationServiceMoq.Object);

            // act
            var result = await sut.Delete(make.Id);

            // assert
            Assert.Equal(ResultCode.BadRequest, result);
        }
    }
}
