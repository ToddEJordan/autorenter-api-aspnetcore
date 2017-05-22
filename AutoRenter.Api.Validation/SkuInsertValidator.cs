using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public class SkuInsertValidator : AbstractValidator<Sku>, IValidator
    {
        public SkuInsertValidator()
        {
            RuleFor(m => m.MakeId).NotNull();
            RuleFor(m => m.ModelId).NotNull();
            RuleFor(m => m.Year).NotNull();
            RuleFor(m => m.Color).NotNull();
        }
    }
}
