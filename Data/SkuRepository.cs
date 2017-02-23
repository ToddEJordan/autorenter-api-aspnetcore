using AutoRenter.API.Domain;

namespace AutoRenter.API.Data
{
    public class SkuRepository : EntityBaseRepository<Sku>, ISkuRepository
    {
        public SkuRepository(AutoRenterContext context) : base(context)
        {
        }
    }
}