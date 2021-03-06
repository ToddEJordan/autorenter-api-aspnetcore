﻿using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Domain.Validation
{
    public class VehicleInsertValidator : AbstractValidator<Vehicle>, IValidator<Vehicle>
    {
        public VehicleInsertValidator()
        {
            RuleFor(m => m.Vin).NotNull();
            RuleFor(m => m.MakeId).NotNull();
            RuleFor(m => m.ModelId).NotNull();
            RuleFor(m => m.Year).NotNull();
        }
    }
}
