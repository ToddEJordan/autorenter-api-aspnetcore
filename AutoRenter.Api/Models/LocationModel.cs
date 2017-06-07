using System;

namespace AutoRenter.Api.Models
{
    public class LocationModel
    {
        public Guid Id { get; set; }

        public string SiteId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public int VehicleCount { get; set; }
    }
}
