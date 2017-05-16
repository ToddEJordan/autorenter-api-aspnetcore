using System.Threading.Tasks;
using AutoRenter.Api.Domain;

namespace AutoRenter.Api.Commands.Interfaces
{
    public interface IDeleteCommand<T>
        where T : class, IEntity
    {
        Task<ResultCode> Execute(T entity);
    }
}
