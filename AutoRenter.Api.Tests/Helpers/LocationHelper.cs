using System.Collections.Generic;
using System.Linq;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class LocationHelper
    {
        internal static Location Get()
        {
            return TestLocation();
        }

        internal static IEnumerable<Location> GetMany()
        {
            return new[] { TestLocation() };
        }

        private static Location TestLocation()
        {
            return new Location()
            {
                Id = IdentifierHelper.LocationId,
                City = "Indianapolis",
                StateCode = "IN",
                Name = "Indy Location",
                SiteId = "1",
                Vehicles = VehicleHelper.GetMany().ToList()
            };
        }
    }
}
