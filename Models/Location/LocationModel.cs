using System;
using System.Collections.Generic;
using AutoRenter.API.Models.Vehicle;

namespace AutoRenter.API.Models.Location
{
    public class LocationModel
    {
        public Guid Id { get; set; }
        public string SiteId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }

        public int VehicleCount => Vehicles.Count;

        public ICollection<VehicleModel> Vehicles { get; set; } = new List<VehicleModel>();
    }
}