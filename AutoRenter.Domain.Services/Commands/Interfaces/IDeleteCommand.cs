using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services.Commands.Interfaces
{
    public interface IDeleteCommand<T>
        where T : class, IEntity
    {
        Task<ResultCode> Execute(T entity);
    }
}
