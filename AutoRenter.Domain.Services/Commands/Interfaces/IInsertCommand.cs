using System;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services.Commands.Interfaces
{
    public interface IInsertCommand<T>
        where T: class, IEntity
    {
        Task<Result<Guid>> Execute(T entity);
    }
}
