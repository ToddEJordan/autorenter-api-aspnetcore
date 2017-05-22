using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public class LocationInsertValidator : AbstractValidator<Location>, IValidator<Location>
    {
        public LocationInsertValidator()
        {
            RuleFor(m => m.SiteId).NotNull();
            RuleFor(m => m.Name).NotNull();
            RuleFor(m => m.City).NotNull();
            RuleFor(m => m.StateCode).NotNull();
        }
    }
}
