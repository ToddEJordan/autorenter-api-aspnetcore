using System.Collections.Generic;
using System.Linq;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class LocationModelHelper
    {
        internal static LocationModel Get()
        {
            return TestLocationModel();
        }

        internal static IEnumerable<LocationModel> GetMany()
        {
            return new[] { TestLocationModel() };
        }

        private static LocationModel TestLocationModel()
        {
            return new LocationModel()
            {
                Id = IdentifierHelper.LocationId,
                City = "Indianapolis",
                StateCode = "IN",
                Name = "Indy LocationModel",
                SiteId = "1",
                VehicleCount = 1
            };
        }
    }
}
