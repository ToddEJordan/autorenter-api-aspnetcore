using AutoRenter.Api.Domain;

namespace AutoRenter.Api.Data
{
    public class SkuRepository : EntityBaseRepository<Sku>, ISkuRepository
    {
        public SkuRepository(AutoRenterContext context) : base(context)
        {
        }
    }
}