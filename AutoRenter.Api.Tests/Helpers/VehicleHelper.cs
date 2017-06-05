using System.Collections.Generic;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class VehicleHelper
    {
        internal static Vehicle Get()
        {
            return TestVehicle();
        }

        internal static IEnumerable<Vehicle> GetMany()
        {
            return new[] { TestVehicle() };
        }

        private static Vehicle TestVehicle()
        {
            var make = MakeHelper.Get();
            var model = ModelHelper.Get();

            return new Vehicle()
            {
                Id = IdentifierHelper.VehicleId,
                Color = "blue",
                IsRentToOwn = false,
                LocationId = IdentifierHelper.LocationId,
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
