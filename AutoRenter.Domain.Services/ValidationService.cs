using System.Threading.Tasks;
using FluentValidation;
using AutoRenter.Api.Validation;
using AutoRenter.Domain.Interfaces;

namespace AutoRenter.Domain.Services
{
    public class ValidationService : IValidationService, IDomainService
    {
        public async Task<bool> IsValidInsert<T>(T entity)
        {
            var validator = ValidatorFactory.GetInsertInstance<T>();
            return await IsValid(validator, entity);
        }

        public async Task<bool> IsValidUpdate<T>(T entity)
        {
            var validator = ValidatorFactory.GetUpdateInstance<T>();
            return await IsValid(validator, entity);
        }

        public async Task<bool> IsValidDelete<T>(T entity)
        {
            var validator = ValidatorFactory.GetDeleteInstance<T>();
            return await IsValid(validator, entity);
        }

        private async Task<bool> IsValid<T>(IValidator validator, T entity)
        {
            var result = await validator.ValidateAsync(entity);
            return result.IsValid;
        }
    }
}
