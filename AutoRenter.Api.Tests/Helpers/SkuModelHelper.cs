using System.Collections.Generic;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class SkuModelHelper
    {
        internal static SkuModel Get()
        {
            return TestSkuModel();
        }

        internal static IEnumerable<SkuModel> GetMany()
        {
            return new[] { TestSkuModel() };
        }

        private static SkuModel TestSkuModel()
        {
            var make = MakeHelper.Get();
            var model = ModelHelper.Get();
            return new SkuModel
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
