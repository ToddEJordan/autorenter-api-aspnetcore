using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public class SkuDeleteValidator : AbstractValidator<Sku>, IValidator
    {
        public SkuDeleteValidator()
        {
            RuleFor(m => m.Id).NotNull();
        }
    }
}
