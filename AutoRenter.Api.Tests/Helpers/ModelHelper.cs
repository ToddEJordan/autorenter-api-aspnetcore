using System.Collections.Generic;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class ModelHelper
    {
        internal Model Get()
        {
            return TestModel();
        }

        internal IEnumerable<Model> GetMany()
        {
            return new[] { TestModel() };
        }

        private Model TestModel()
        {
            return new Model()
            {
                Id = new IdentifierHelper().ModelId,
                ExternalId = "ModelId",
                Name = "ModelName"
            };
        }
    }
}
