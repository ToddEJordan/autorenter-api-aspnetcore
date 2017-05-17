using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services.Tests
{
    public class LocationServiceTests : IDisposable
    {
        AutoRenterContext context;

        public LocationServiceTests()
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
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();
            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.NotFound));
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);
            
            // act
            var result = await sut.GetAll();

            // assert
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async void GetAll_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();
            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.NotFound));
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var allLocation = await context.Set<Location>().ToListAsync();
            context.Locations.RemoveRange(allLocation);
            await context.SaveChangesAsync();

            // act
            var result = await sut.GetAll();

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async void GetAll_ReturnsVehicles()
        {
            // arrange
            var vehicle = await context.Vehicles.FirstAsync();

            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();
            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.Success, new[] { vehicle }));
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            // act
            var result = await sut.GetAll();

            // assert
            Assert.NotEmpty(result.Data.First().Vehicles);
        }

        [Fact]
        public async void Get_ReturnsData()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();
            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.NotFound));
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            // act
            var result = await sut.Get(new Guid("c0b694ec-3352-43e3-9f22-77c87fe83d48"));

            // assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async void Get_WhenNotFoundReturnsNotFound()
        {
            // arrange
            var targetId = new Guid("c0b694ec-3352-43e3-9f22-77c87fe83d48");
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();
            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.NotFound));
                
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var targetEntity = await context.FindAsync<Location>(targetId);
            var removeResult = context.Remove(targetEntity);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Get(targetId);

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async void Get_ReturnsVehicles()
        {
            // arrange
            var vehicle = await context.Vehicles.FirstAsync();

            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();
            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.Success, new[] { vehicle }));
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            // act
            var result = await sut.GetAll();

            // assert
            Assert.NotEmpty(result.Data.First().Vehicles);
        }

        [Fact]
        public async void Insert_Succeeds()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Location>()))
                .ReturnsAsync(() => true);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            context.Locations.Remove(location);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(location);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Insert_WhenDuplicateReturnsConflict()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Location>()))
                .ReturnsAsync(() => true);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();

            // act
            var result = await sut.Insert(location);

            // assert
            Assert.Equal(ResultCode.Conflict, result.ResultCode);
        }
        
        [Fact]
        public async void Insert_ReturnsId()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Location>()))
                .ReturnsAsync(() => true);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            context.Locations.Remove(location);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(location);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Insert_InvalidReturnsBadRequest()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidInsert(It.IsAny<Location>()))
                .ReturnsAsync(() => false);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            context.Locations.Remove(location);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Insert(location);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Update_Succeeds()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Location>()))
                .ReturnsAsync(() => true);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            location.City = "New Orleans";

            // act
            var result = await sut.Update(location);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void Update_ReturnsId()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Location>()))
                .ReturnsAsync(() => true);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            location.City = "New Orleans";

            // act
            var result = await sut.Update(location);

            // assert
            Assert.NotEqual(Guid.Empty, result.Data);
        }

        [Fact]
        public async void Update_InvalidReturnsBadRequest()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidUpdate(It.IsAny<Location>()))
                .ReturnsAsync(() => false);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            location.City = "New Orleans";

            // act
            var result = await sut.Insert(location);

            // assert
            Assert.Equal(ResultCode.BadRequest, result.ResultCode);
        }

        [Fact]
        public async void Delete_Succeeds()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Location>()))
                .ReturnsAsync(() => true);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();

            // act
            var result = await sut.Delete(location.Id);

            // assert
            Assert.Equal(ResultCode.Success, result);
        }

        [Fact]
        public async void Delete_NotFoundWhenNotFound()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Location>()))
                .ReturnsAsync(() => true);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            context.Locations.Remove(location);
            await context.SaveChangesAsync();

            // act
            var result = await sut.Delete(location.Id);

            // assert
            Assert.Equal(ResultCode.NotFound, result);
        }

        [Fact]
        public async void Delete_InvalidReturnsBadRequest()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            validationServiceMoq.Setup(x => x.IsValidDelete(It.IsAny<Location>()))
                .ReturnsAsync(() => false);
            var vehicleServiceMoq = new Mock<IVehicleService>();
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();

            // act
            var result = await sut.Delete(location.Id);

            // assert
            Assert.Equal(ResultCode.BadRequest, result);
        }

        [Fact]
        public async void AddVehicle_Succeeds()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();
            
            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            var vehicle = location.Vehicles.First();
            location.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();

            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.Success, location.Vehicles));

            // act
            var result = await sut.AddVehicle(location.Id, vehicle);

            // assert
            Assert.Equal(ResultCode.Success, result);
        }

        [Fact]
        public async void AddVehicle_NotFoundWhenLocationNotFound()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();

            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            var vehicle = location.Vehicles.First();
            location.Vehicles.Remove(vehicle);
            await context.SaveChangesAsync();

            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.NotFound));

            // act
            var result = await sut.AddVehicle(Guid.NewGuid(), vehicle);

            // assert
            Assert.Equal(ResultCode.NotFound, result);
        }

        [Fact]
        public async void AddVehicle_ConflictWhenDuplicate()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();

            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();
            var vehicle = location.Vehicles.First();

            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.Success, new[] { vehicle }));

            // act
            var result = await sut.AddVehicle(location.Id, vehicle);

            // assert
            Assert.Equal(ResultCode.Conflict, result);
        }

        [Fact]
        public async void GetVehicles_Succeeds()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();

            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();

            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.Success, location.Vehicles));

            // act
            var result = await sut.GetVehicles(location.Id);

            // assert
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async void GetVehicles_ReturnsVehicles()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();

            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();

            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.Success, location.Vehicles));

            // act
            var result = await sut.GetVehicles(location.Id);

            // assert
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async void GetVehicles_InvalidLocationReturnsNotFound()
        {
            // arrange
            var validationServiceMoq = new Mock<IValidationService>();
            var vehicleServiceMoq = new Mock<IVehicleService>();

            var sut = new LocationService(context, vehicleServiceMoq.Object, validationServiceMoq.Object);

            var location = context.Locations.First();

            vehicleServiceMoq.Setup(x => x.GetByLocationId(It.IsAny<Guid>()))
                .ReturnsAsync(() => new Result<IEnumerable<Vehicle>>(ResultCode.NotFound));

            // act
            var result = await sut.GetVehicles(Guid.NewGuid());

            // assert
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }
    }
}
