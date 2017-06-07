using System;

namespace AutoRenter.Api.Models
{
    public class VehicleModel
    {
        public Guid Id { get; set; }

        public string Vin { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public int Year { get; set; }
        public int Miles { get; set; }
        public string Color { get; set; }
        public bool IsRentToOwn { get; set; }
        public string Image { get; set; }

        public Guid LocationId { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
    }
}
