using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface IStateService
    {
        Task<Result<IEnumerable<State>>> GetAll();
    }
}
