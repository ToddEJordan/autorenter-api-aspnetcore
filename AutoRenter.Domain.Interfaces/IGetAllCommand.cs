using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface IGetAllCommand<T>
        where T : class, IEntity
    {
        Task<Result<IEnumerable<T>>> Execute();
    }
}
