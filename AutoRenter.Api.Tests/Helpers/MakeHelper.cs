using System.Collections.Generic;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal class MakeHelper
    {
        public Make Get()
        {
            return TestMake();
        }

        public IEnumerable<Make> GetMany()
        {
            return new[] { TestMake() };
        }

        private Make TestMake()
        {
            return new Make
            {
                Id = new IdentifierHelper().MakeId,
                ExternalId = "MakeId",
                Name = "MakeName"
            };
        }
    }
}
