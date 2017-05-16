using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
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
