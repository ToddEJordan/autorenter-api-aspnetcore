using AutoRenter.Api.Domain;
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

    public class LocationUpdateValidator : AbstractValidator<Location>, IValidator<Location>
    {
        public LocationUpdateValidator()
        {
            RuleFor(m => m.Id).NotNull();
            RuleFor(m => m.SiteId).NotNull();
            RuleFor(m => m.Name).NotNull();
            RuleFor(m => m.City).NotNull();
            RuleFor(m => m.StateCode).NotNull();
        }
    }

    public class LocationDeleteValidator : AbstractValidator<Location>, IValidator<Location>
    {
        public LocationDeleteValidator()
        {
            RuleFor(m => m.Id).NotNull();
        }
    }
}
