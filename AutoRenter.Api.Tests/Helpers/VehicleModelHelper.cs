using System.Collections.Generic;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class VehicleModelHelper
    {
        internal VehicleModel Get()
        {
            return TestVehicle();
        }

        internal IEnumerable<VehicleModel> GetMany()
        {
            return new[] { TestVehicle() };
        }

        private VehicleModel TestVehicle()
        {
            var make = new MakeHelper().Get();
            var model = new ModelHelper().Get();

            return new VehicleModel()
            {
                Id = new IdentifierHelper().VehicleId,
                Color = "blue",
                IsRentToOwn = false,
                LocationId = new IdentifierHelper().LocationId,
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
