using System.Collections.Generic;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class VehicleModelHelper
    {
        internal static VehicleModel Get()
        {
            return TestVehicle();
        }

        internal static IEnumerable<VehicleModel> GetMany()
        {
            return new[] { TestVehicle() };
        }

        private static VehicleModel TestVehicle()
        {
            var make = MakeHelper.Get();
            var model = ModelHelper.Get();

            return new VehicleModel()
            {
                Id = IdentifierHelper.VehicleId,
                Color = "blue",
                IsRentToOwn = false,
                LocationId = IdentifierHelper.LocationId,
                MakeId = make.ExternalId,
                Make = make.Name,
                ModelId = model.ExternalId,
                Model = model.Name,
                Miles = 1000,
                Vin = "0XJ9TTYZ6N7M81234",
                Year = 2016
            };
        }
    }
}
