using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface ISkuService
    {
        Task<Result<IEnumerable<Sku>>> GetAll();
    }
}
