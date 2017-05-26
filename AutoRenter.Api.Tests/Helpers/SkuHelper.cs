using System.Collections.Generic;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class SkuHelper
    {
        internal Sku Get()
        {
            return TestSku();
        }

        internal IEnumerable<Sku> GetMany()
        {
            return new[] { TestSku() };
        }

        private Sku TestSku()
        {
            var make = new MakeHelper().Get();
            var model = new ModelHelper().Get();
            return new Sku
            {
                Id = new IdentifierHelper().SkuId,
                MakeId = make.ExternalId,
                ModelId = model.ExternalId,
                Year = 2016,
                Color = "Black"
            };
        }
    }
}
