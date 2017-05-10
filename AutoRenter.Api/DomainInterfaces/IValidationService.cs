using System.Threading.Tasks;

namespace AutoRenter.Api.DomainInterfaces
{
    public interface IValidationService
    {
        Task<bool> IsValidInsert<T>(T entity);
        Task<bool> IsValidUpdate<T>(T entity);
        Task<bool> IsValidDelete<T>(T entity);
    }
}
