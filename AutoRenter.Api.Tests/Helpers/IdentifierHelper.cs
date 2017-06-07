using System;

namespace AutoRenter.Api.Tests.Helpers
{
    internal static class IdentifierHelper
    {
        static internal Guid MakeId => new Guid("4f307da8-6bb6-404d-b214-38028a9be953");
        static internal Guid ModelId => new Guid("aa23f971-b3be-43c7-8073-ea2c36e3a6fe");
        static internal Guid VehicleId => new Guid("52731074-43be-4e67-8374-17ee4ec3369d");
        static internal Guid LocationId => new Guid("a341dc33-fe65-4c8d-a7b5-16be1741c02e");
        static internal Guid SkuId => new Guid("b19a9a7a-3631-427e-90ad-777f3d2b2535");
    }
}
