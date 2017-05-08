using AutoRenter.Api.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoRenter.Api.DomainInterfaces
{
    public interface IServiceBase<T>
    {
        Result<IEnumerable<T>> GetAll();
        Task<Result<T>> Get(Guid id);
        Task<ResultCode> Delete(Guid id);
        Task<Result<Guid>> Insert(T entity);
        Task<Result<Guid>> Update(T entity);
        Task<Result<Guid>> Upsert(T entity);
    }
}
