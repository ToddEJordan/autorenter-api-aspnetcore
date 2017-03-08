using AutoRenter.Api.Domain;

namespace AutoRenter.Api.Data
{
    public class VehicleRepository : EntityBaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(AutoRenterContext context) : base(context)
        {
        }
    }
}