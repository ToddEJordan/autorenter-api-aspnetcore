using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Api.Domain;

namespace AutoRenter.Api.DomainInterfaces
{
    public interface ILocationService
    {
        Result<IEnumerable<Location>> GetAll();
        Task<Result<Location>> Get(Guid id);
        Task<Result<Guid>> Insert(Location location);
        Task<Result<Guid>> Update(Location location);
        Task<ResultCode> Delete(Guid id);
        Task<ResultCode> AddVehicle(Guid locationId, Vehicle vehicle);
        Task<Result<IEnumerable<Vehicle>>> GetVehicles(Guid locationId);
    }
}
