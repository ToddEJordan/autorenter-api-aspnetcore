using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Moq;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Api.Controllers;

namespace AutoRenter.Api.OldTests.Controllers
{
    [TestClass]
    public class LocationsControllerTests
    {
        [TestMethod]
        public async void GetAllReturnsData()
        {
            // arrange
            var goodLocations = GoodArrangement();
            var moqResult = new Result<IEnumerable<Location>>(ResultCode.Success, goodLocations);

            var locationServiceMoq = new Mock<ILocationService>();
            locationServiceMoq.Setup(x => x.GetAll())
                .ReturnsAsync(() => moqResult);

            var locationsController = new LocationsController(locationServiceMoq.Object);

            // act
            var result = await locationsController.GetAll();
            var okResult = result as OkObjectResult;
            var data = okResult.Value as Result<IEnumerable<Location>>;

            // assert
            Assert.IsNotNull(okResult);
            Assert.Equals(200, okResult.StatusCode);
            Assert.IsNotNull(data);
        }

        private ICollection<Location> GoodArrangement()
        {
            var locationId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();
            var makeId = Guid.NewGuid();
            var modelId = Guid.NewGuid();

            var make = new Make()
            {
                Id = makeId,
                ExternalId = "MakeId",
                Name = "MakeName"
            };

            var model = new Model()
            {
                Id = modelId,
                ExternalId = "ModelId",
                Name = "ModelName"
            };

            var vehicles = new List<Vehicle>()
            {
                new Vehicle()
                {
                    Id = Guid.NewGuid(),
                    Color = "blue",
                    IsRentToOwn = false,
                    LocationId = locationId,
                    MakeId = make.ExternalId,
                    Make = make,
                    ModelId = model.ExternalId,
                    Model = model,
                    Miles = 1000,
                    Vin = "0XJ9TTYZ6N7M81234",
                    Year = 2016
                }
            };

            return new List<Location>()
            {
                new Location()
                {
                    Id = locationId,
                    City = "Indianapolis",
                    StateCode = "IN",
                    Name = "Indy Location",
                    SiteId = "1",
                    Vehicles = vehicles
                }
            };
        }

    }
}
