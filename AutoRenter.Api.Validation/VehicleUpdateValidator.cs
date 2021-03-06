﻿using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Domain.Validation
{
    public class VehicleUpdateValidator : AbstractValidator<Vehicle>, IValidator<Vehicle>
    {
        public VehicleUpdateValidator()
        {
            RuleFor(m => m.Id).NotNull();
            RuleFor(m => m.Vin).NotNull();
            RuleFor(m => m.MakeId).NotNull();
            RuleFor(m => m.ModelId).NotNull();
            RuleFor(m => m.Year).NotNull();
        }
    }
}
