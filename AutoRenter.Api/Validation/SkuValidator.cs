using AutoRenter.Api.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        
    public class SkuUpdateValidator : AbstractValidator<Sku>, IValidator
    {
        public SkuUpdateValidator()
        {
            RuleFor(m => m.Id).NotNull();
            RuleFor(m => m.MakeId).NotNull();
            RuleFor(m => m.ModelId).NotNull();
            RuleFor(m => m.Year).NotNull();
            RuleFor(m => m.Color).NotNull();
        }
    }

    public class SkuDeleteValidator : AbstractValidator<Sku>, IValidator
    {
        public SkuDeleteValidator()
        {
            RuleFor(m => m.Id).NotNull();
        }
    }
}
