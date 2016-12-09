using AutoRenter.API.Domain;

namespace AutoRenter.API.Data
{
    public class LocationRepository : EntityBaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(AutoRenterContext context) : base(context)
        {
        }
    }
}