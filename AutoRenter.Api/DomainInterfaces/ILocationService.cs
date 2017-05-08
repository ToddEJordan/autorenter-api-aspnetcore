using AutoRenter.Api.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoRenter.Api.DomainInterfaces
{
    public interface ILocationService : IServiceBase<Location>
    {
        Task<Result<IEnumerable<Vehicle>>> GetVehicles(Guid locationId);
    }
}
