using System;
using System.Collections.Generic;

namespace AutoRenter.API.Entities
{
    public class Location : IEntityBase
    {
        public Guid Id { get; set; }
        public string SiteId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}