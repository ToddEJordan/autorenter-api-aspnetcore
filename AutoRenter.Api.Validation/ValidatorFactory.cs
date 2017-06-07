using System;
using System.Collections.Generic;
using FluentValidation;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Validation
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly Dictionary<Type, IValidator> insertValidators = new Dictionary<Type, IValidator>
        {
            { typeof(Location), new LocationInsertValidator() },
            { typeof(Vehicle), new VehicleInsertValidator() },
            { typeof(LogEntry), new LogValidator() }
        };

        private readonly Dictionary<Type, IValidator> updateValidators = new Dictionary<Type, IValidator>
        {
            { typeof(Location), new LocationUpdateValidator() },
            { typeof(Vehicle), new VehicleUpdateValidator() }
        };

        private readonly Dictionary<Type, IValidator> deleteValidators = new Dictionary<Type, IValidator>
        {
            { typeof(Location), new LocationDeleteValidator() },
            { typeof(Vehicle), new VehicleDeleteValidator() }
        };

        public IValidator GetInsertInstance<T>()
        {
            if (!insertValidators.ContainsKey(typeof(T)))
            {
                return null;
            }

            return insertValidators[typeof(T)];
        }

        public IValidator GetUpdateInstance<T>()
        {
            if (!updateValidators.ContainsKey(typeof(T)))
            {
                return null;
            }

            return updateValidators[typeof(T)];
        }

        public IValidator GetDeleteInstance<T>()
        {
            if (!deleteValidators.ContainsKey(typeof(T)))
            {
                return null;
            }

            return deleteValidators[typeof(T)];
        }
    }
}
