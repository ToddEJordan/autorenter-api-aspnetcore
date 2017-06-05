using System.Collections.Generic;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class MakeModelHelper
    {
        public static MakeModel Get()
        {
            return TestMakeModel();
        }

        public static IEnumerable<MakeModel> GetMany()
        {
            return new[] { TestMakeModel() };
        }

        private static MakeModel TestMakeModel()
        {
            return new MakeModel
            {
                Id = "MakeId",
                Name = "MakeName"
            };
        }
    }
}
