using System.Collections.Generic;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class VehicleHelper
    {
        internal Vehicle Get()
        {
            return TestVehicle();
        }

        internal IEnumerable<Vehicle> GetMany()
        {
            return new[] { TestVehicle() };
        }

        private Vehicle TestVehicle()
        {
            var make = new MakeHelper().Get();
            var model = new ModelHelper().Get();

            return new Vehicle()
            {
                Id = new IdentifierHelper().VehicleId,
                Color = "blue",
                IsRentToOwn = false,
                LocationId = new IdentifierHelper().LocationId,
                MakeId = make.ExternalId,
                Make = make,
                ModelId = model.ExternalId,
                Model = model,
                Miles = 1000,
                Vin = "0XJ9TTYZ6N7M81234",
                Year = 2016
            };
        }
    }
}
