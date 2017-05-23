using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface IStateService
    {
        Task<Result<State>> Get(Guid id);
        Task<Result<IEnumerable<State>>> GetAll();
    }
}
