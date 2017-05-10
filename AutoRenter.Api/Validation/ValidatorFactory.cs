using AutoRenter.Api.Domain;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace AutoRenter.Api.Validation
{
    public static class ValidatorFactory
    {
        private static readonly Dictionary<Type, IValidator> insertValidators = new Dictionary<Type, IValidator>()
        {
            { typeof(Location), new LocationInsertValidator() as IValidator },
            { typeof(Vehicle), new VehicleInsertValidator() as IValidator },
            { typeof(Sku), new SkuInsertValidator() as IValidator },
        };

        private static readonly Dictionary<Type, IValidator> updateValidators = new Dictionary<Type, IValidator>()
        {
            { typeof(Location), new LocationUpdateValidator() as IValidator },
            { typeof(Vehicle), new VehicleUpdateValidator() as IValidator },
            { typeof(Sku), new SkuUpdateValidator() as IValidator }
        };

        private static readonly Dictionary<Type, IValidator> deleteValidators = new Dictionary<Type, IValidator>()
        {
            { typeof(Location), new LocationDeleteValidator() as IValidator },
            { typeof(Vehicle), new VehicleDeleteValidator() as IValidator },
            { typeof(Sku), new SkuDeleteValidator() as IValidator }
        };

        public static IValidator GetInsertInstance<T>()
        {
            if (!insertValidators.ContainsKey(typeof(T)))
            {
                return null;
            }

            return insertValidators[typeof(T)];
        }

        public static IValidator GetUpdateInstance<T>()
        {
            if (!updateValidators.ContainsKey(typeof(T)))
            {
                return null;
            }

            return updateValidators[typeof(T)];
        }

        public static IValidator GetDeleteInstance<T>()
        {
            if (!updateValidators.ContainsKey(typeof(T)))
            {
                return null;
            }

            return updateValidators[typeof(T)];
        }
    }
}
