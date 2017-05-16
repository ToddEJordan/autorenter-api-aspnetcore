using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Api.Domain;

namespace AutoRenter.Api.DomainInterfaces
{
    public interface IVehicleService
    {
        Task<Result<IEnumerable<Vehicle>>> GetAll();
        Task<Result<Vehicle>> Get(Guid id);
        Task<Result<Guid>> Insert(Vehicle vehicle);
        Task<Result<Guid>> Update(Vehicle vehicle);
        Task<ResultCode> Delete(Guid id);
    }
}
