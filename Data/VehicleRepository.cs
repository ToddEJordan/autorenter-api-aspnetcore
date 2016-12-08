using AutoRenter.API.Entities;

namespace AutoRenter.API.Data
{
    public class VehicleRepository : EntityBaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(AutoRenterContext context) : base(context)
        {

        }
    }
}