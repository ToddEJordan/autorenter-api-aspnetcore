using System.Linq;
using DomainLocation = AutoRenter.Domain.Models.Location;

namespace AutoRenter.Api.Models
{
    public class LocationModel : DomainLocation
    {
        public int VehicleCount
        {
            get
            {
                if (base.Vehicles == null || !base.Vehicles.Any())
                {
                    return 0;
                }

                return base.Vehicles.Count;
            }
        }
    }
}
