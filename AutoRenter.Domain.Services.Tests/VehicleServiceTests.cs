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
    public class VehicleServiceTests : IDisposable
    {
        AutoRenterContext context;

        public VehicleServiceTests()
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
            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));
            
            var sut = new VehicleService(context, 
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
            var allVehicle = await context.Set<Vehicle>().ToListAsync();
            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            context.Vehicles.RemoveRange(allVehicle);
            await context.SaveChangesAsync();

            // act
            var result = await sut.GetAll();

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async void GetAll_ReturnsMake()
        {
            // arrange
            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.GetAll();
            var sample = result.Data.FirstOrDefault();

            // assert
            Assert.NotNull(sample.Make);
        }

        [Fact]
        public async void GetAll_ReturnsModel()
        {
            // arrange
            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.GetAll();
            var sample = result.Data.FirstOrDefault();

            // assert
            Assert.NotNull(sample.Model);
        }

        [Fact]
        public async void Get_ReturnsData()
        {
            // arrange
            var targetId = context.Vehicles.FirstOrDefault().Id;
            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
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
            var targetId = context.Vehicles.FirstOrDefault().Id;
            var targetEntity = await context.FindAsync<Vehicle>(targetId);

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
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
        public async void Get_ReturnsMake()
        {
            // arrange
            var targetId = context.Vehicles.FirstOrDefault().Id;
            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.NotNull(result.Data.Make);
        }

        [Fact]
        public async void Get_ReturnsModel()
        {
            // arrange
            var targetId = context.Vehicles.FirstOrDefault().Id;
            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.NotNull(result.Data.Model);
        }

        [Fact]
        public async void Insert_Succeeds()
        {
            // arrange
            var vehicle = context.Vehicles.First();

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Vehicle>()))
                .ReturnsAsync(true);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(vehicle);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Insert_WhenDuplicateReturnsConflict()
        {
            // arrange
            var vehicle = context.Vehicles.First();

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Vehicle>()))
                .ReturnsAsync(true);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.Insert(vehicle);

            // assert
            Assert.Equal(ResultCode.Conflict, result.ResultCode);
        }
        
        [Fact]
        public async void Insert_ReturnsId()
        {
            // arrange
            var vehicle = context.Vehicles.First();

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Vehicle>()))
                .ReturnsAsync(true);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(vehicle);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Insert_InvalidReturnsBadRequest()
        {
            // arrange
            var vehicle = context.Vehicles.First();

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Vehicle>()))
                .ReturnsAsync(false);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(vehicle);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Update_Succeeds()
        {
            // arrange
            var vehicle = context.Vehicles.First();
            vehicle.Color = "Orange";

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Vehicle>()))
                .ReturnsAsync(true);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.Update(vehicle);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Update_ReturnsId()
        {
            // arrange
            var vehicle = context.Vehicles.First();
            vehicle.Color = "Orange";

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Vehicle>()))
                .ReturnsAsync(true);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.Update(vehicle);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Update_InvalidReturnsBadRequest()
        {
            // arrange
            var vehicle = context.Vehicles.First();
            vehicle.Color = "Orange";

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Vehicle>()))
                .ReturnsAsync(false);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.Insert(vehicle);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Delete_Succeeds()
        {
            // arrange
            var vehicle = context.Vehicles.First();

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Vehicle>()))
                .ReturnsAsync(true);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.Delete(vehicle.Id);

            // assert
            Assert.Equal(ResultCode.Success, result);
        }

        [Fact]
        public async void Delete_NotFoundWhenNotFound()
        {
            // arrange
            var vehicle = context.Vehicles.First();

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Vehicle>()))
                .ReturnsAsync(true);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            context.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Delete(vehicle.Id);

            // assert
            Assert.Equal(ResultCode.NotFound, result);
        }

        [Fact]
        public async void Delete_InvalidReturnsBadRequest()
        {
            // arrange
            var vehicle = context.Vehicles.First();

            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();

            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Vehicle>()))
                .ReturnsAsync(false);

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.Delete(vehicle.Id);

            // assert
            Assert.Equal(ResultCode.BadRequest, result);
        }

        [Fact]
        public async void GetByLocationId_ReturnsData()
        {
            // arrange
            var targetId = context.Locations.FirstOrDefault().Id;
            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            // act
            var result = await sut.GetByLocationId(targetId);

            // assert
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async void GetByLocationId_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var targetId = context.Locations.FirstOrDefault().Id;
            var targetVehicles = context.Vehicles.Where(x => x.LocationId == targetId);
            ICommandFactory<Vehicle> commandFactory = new CommandFactory<Vehicle>();
            var validationServiceMoq = new Mock<IValidationService>();

            var makeServiceMoq = new Mock<IMakeService>();
            makeServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Make>(ResultCode.Success, context.Makes.First()));

            var modelServiceMoq = new Mock<IModelService>();
            modelServiceMoq.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new Result<Model>(ResultCode.Success, context.Models.First()));

            var sut = new VehicleService(context,
                commandFactory,
                validationServiceMoq.Object,
                makeServiceMoq.Object,
                modelServiceMoq.Object);

            context.Vehicles.RemoveRange(targetVehicles);
            await context.SaveChangesAsync();

            // act
            var result = await sut.GetByLocationId(targetId);

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }
    }
}
