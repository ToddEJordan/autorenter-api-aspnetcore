using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface IModelService
    {
        Task<Result<IEnumerable<Model>>> GetAll();
        Task<Result<Model>> Get(Guid id);
        Task<Result<Model>> Get(string id);
        Task<Result<Guid>> Insert(Model Model);
        Task<Result<Guid>> Update(Model Model);
        Task<ResultCode> Delete(Guid id);
    }
}
