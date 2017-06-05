using System.Collections.Generic;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class ModelModelHelper
    {
        internal static ModelModel Get()
        {
            return TestModelModel();
        }

        internal static IEnumerable<ModelModel> GetMany()
        {
            return new[] { TestModelModel() };
        }

        private static ModelModel TestModelModel()
        {
            return new ModelModel()
            {
                Id = "ModelModelId",
                Name = "ModelModelName"
            };
        }
    }
}
