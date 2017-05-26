using System.Collections.Generic;
using System.Linq;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class LocationHelper
    {
        internal Location Get()
        {
            return TestLocation();
        }

        internal IEnumerable<Location> GetMany()
        {
            return new[] { TestLocation() };
        }

        private Location TestLocation()
        {
            return new Location()
            {
                Id = new IdentifierHelper().LocationId,
                City = "Indianapolis",
                StateCode = "IN",
                Name = "Indy Location",
                SiteId = "1",
                Vehicles = new VehicleHelper().GetMany().ToList()
            };
        }
    }
}
