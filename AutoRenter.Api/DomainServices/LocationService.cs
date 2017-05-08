using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using AutoRenter.Api.DomainInterfaces;
using AutoRenter.Api.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRenter.Api.DomainServices
{
    public class LocationService : ServiceBase<Location>, ILocationService
    {
        private readonly AbstractValidator<Location> validator;

        private readonly IVehicleService vehicleService;
        public LocationService(AutoRenterContext context, IVehicleService vehicleService)
            : base(context)
        {
            this.vehicleService = vehicleService;
        }

        public async Task<Result<IEnumerable<Vehicle>>> GetVehicles(Guid locationId)
        {
            var locationResult = await Get(locationId);
            if (locationResult.ResultCode == ResultCode.Success)
            {
                var vehicles = locationResult.Data.Vehicles;
                if (vehicles == null || !vehicles.Any())
                {
                    return new Result<IEnumerable<Vehicle>>()
                    {
                        Data = null,
                        ResultCode = ResultCode.NotFound
                    };
                }

                return new Result<IEnumerable<Vehicle>>()
                {
                    Data = vehicles,
                    ResultCode = ResultCode.Success
                };
            }

            return new Result<IEnumerable<Vehicle>>()
            {
                Data = null,
                ResultCode = ResultCode.Failed
            };
        }
    }
}