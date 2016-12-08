using System;
using System.Collections.Generic;

namespace AutoRenter.API.Models
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        public string SiteId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }

        public int VehicleCount => Vehicles.Count;

        public ICollection<VehicleDto> Vehicles { get; set; } = new List<VehicleDto>();
    }
}