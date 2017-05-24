using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface IMakeService
    {
        Task<Result<IEnumerable<Make>>> GetAll();
        Task<Result<Make>> Get(Guid id);
        Task<Result<Make>> Get(string id);
        Task<Result<Guid>> Insert(Make make);
        Task<Result<Guid>> Update(Make make);
        Task<ResultCode> Delete(Guid id);
    }
}
