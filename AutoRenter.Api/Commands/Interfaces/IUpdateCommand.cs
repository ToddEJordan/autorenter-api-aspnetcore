using System;
using System.Threading.Tasks;
using AutoRenter.Api.Domain;

namespace AutoRenter.Api.Commands.Interfaces
{
    public interface IUpdateCommand<T>
        where T : class, IEntity
    {
        Task<Result<Guid>> Execute(T entity);
    }
}
