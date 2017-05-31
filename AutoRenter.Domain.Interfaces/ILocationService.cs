using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface ILocationService
    {
        Task<Result<IEnumerable<Location>>> GetAll();
        Task<Result<Location>> Get(Guid id);
        Task<Result<Guid>> Insert(Location location);
        Task<Result<Guid>> Update(Location location);
        Task<ResultCode> Delete(Guid id);
        Task<Result<Guid>> AddVehicle(Guid locationId, Vehicle vehicle);
        Task<Result<IEnumerable<Vehicle>>> GetVehicles(Guid locationId);
    }
}
