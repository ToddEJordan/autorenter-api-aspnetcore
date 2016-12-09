using System;

namespace AutoRenter.API.Domain
{
    public class Vehicle : IEntity
    {
        public string Vin { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Miles { get; set; }
        public string Color { get; set; }
        public bool IsRentToOwn { get; set; }

        public Guid LocationId { get; set; }
        public Location Location { get; set; }
        public Guid Id { get; set; }
    }
}