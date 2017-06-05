using System.Collections.Generic;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class SkuHelper
    {
        internal static Sku Get()
        {
            return TestSku();
        }

        internal static IEnumerable<Sku> GetMany()
        {
            return new[] { TestSku() };
        }

        private static Sku TestSku()
        {
            var make = MakeHelper.Get();
            var model = ModelHelper.Get();
            return new Sku
            {
                Id = IdentifierHelper.SkuId,
                MakeId = make.ExternalId,
                ModelId = model.ExternalId,
                Year = 2016,
                Color = "Black"
            };
        }
    }
}
