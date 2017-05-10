using AutoRenter.Api.DomainInterfaces;
using AutoRenter.Api.Validation;
using FluentValidation;
using System.Threading.Tasks;

namespace AutoRenter.Api.DomainServices
{
    public class ValidationService : IValidationService
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
