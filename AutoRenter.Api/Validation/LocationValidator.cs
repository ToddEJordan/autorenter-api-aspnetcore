using AutoRenter.Api.Models;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public class LocationInsertValidator : AbstractValidator<LocationModel>, IValidator<LocationModel>
    {
        public LocationInsertValidator()
        {
            RuleFor(m => m.SiteId).NotNull();
            RuleFor(m => m.Name).NotNull();
            RuleFor(m => m.City).NotNull();
            RuleFor(m => m.StateCode).NotNull();
        }
    }

    public class LocationUpdateValidator : AbstractValidator<LocationModel>, IValidator<LocationModel>
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

    public class LocationDeleteValidator : AbstractValidator<LocationModel>, IValidator<LocationModel>
    {
        public LocationDeleteValidator()
        {
            RuleFor(m => m.Id).NotNull();
        }
    }
}
