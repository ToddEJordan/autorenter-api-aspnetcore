using System.Collections.Generic;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class MakeHelper
    {
        public static Make Get()
        {
            return TestMake();
        }

        public static IEnumerable<Make> GetMany()
        {
            return new[] { TestMake() };
        }

        private static Make TestMake()
        {
            return new Make
            {
                Id = IdentifierHelper.MakeId,
                ExternalId = "MakeId",
                Name = "MakeName"
            };
        }
    }
}
