using System.Linq;
using DomainLocation = AutoRenter.Api.Domain.Location;

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
