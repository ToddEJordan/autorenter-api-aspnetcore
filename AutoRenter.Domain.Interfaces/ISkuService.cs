using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface ISkuService
    {
        Task<Result<IEnumerable<Sku>>> GetAll();
        Task<Result<Sku>> Get(Guid id);
        Task<Result<Guid>> Insert(Sku sku);
        Task<Result<Guid>> Update(Sku sku);
        Task<ResultCode> Delete(Guid id);
    }
}
