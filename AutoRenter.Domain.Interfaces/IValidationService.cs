using System.Threading.Tasks;

namespace AutoRenter.Domain.Interfaces
{
    public interface IValidationService
    {
        Task<bool> IsValidInsert<T>(T entity);
        Task<bool> IsValidUpdate<T>(T entity);
        Task<bool> IsValidDelete<T>(T entity);
    }
}
