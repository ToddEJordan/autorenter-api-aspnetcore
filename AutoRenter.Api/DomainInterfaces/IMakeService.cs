using AutoRenter.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRenter.Api.DomainInterfaces
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
