using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public interface IValidatorFactory
    {
        IValidator GetInsertInstance<T>();
        IValidator GetUpdateInstance<T>();
        IValidator GetDeleteInstance<T>();
    }
}
