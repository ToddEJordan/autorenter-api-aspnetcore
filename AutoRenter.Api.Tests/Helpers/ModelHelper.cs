using System.Collections.Generic;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class ModelHelper
    {
        internal static Model Get()
        {
            return TestModel();
        }

        internal static IEnumerable<Model> GetMany()
        {
            return new[] { TestModel() };
        }

        private static Model TestModel()
        {
            return new Model()
            {
                Id = IdentifierHelper.ModelId,
                ExternalId = "ModelId",
                Name = "ModelName"
            };
        }
    }
}
