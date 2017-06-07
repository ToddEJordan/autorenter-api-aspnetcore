using System;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface IGetCommand<T>
        where T : class, IEntity
    {
        Task<Result<T>> Execute(Guid id);
    }
}
