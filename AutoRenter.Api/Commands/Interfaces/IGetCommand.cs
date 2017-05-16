using System;
using System.Threading.Tasks;
using AutoRenter.Api.Domain;

namespace AutoRenter.Api.Commands.Interfaces
{
    public interface IGetCommand<T>
        where T : class, IEntity
    {
        Task<Result<T>> Execute(Guid id);
    }
}
