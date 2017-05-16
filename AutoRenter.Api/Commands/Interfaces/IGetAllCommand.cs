using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Api.Domain;

namespace AutoRenter.Api.Commands.Interfaces
{
    public interface IGetAllCommand<T>
        where T : class, IEntity
    {
        Task<Result<IEnumerable<T>>> Execute();
    }
}
