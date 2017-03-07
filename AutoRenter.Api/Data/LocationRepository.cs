using AutoRenter.Api.Domain;

namespace AutoRenter.Api.Data
{
    public class LocationRepository : EntityBaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(AutoRenterContext context) : base(context)
        {
        }
    }
}