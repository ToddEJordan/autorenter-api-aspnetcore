using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using AutoRenter.Api.DomainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRenter.Api.DomainServices
{
    public class VehicleService : ServiceBase<Vehicle>, IVehicleService
    {
        public VehicleService(AutoRenterContext context)
            : base(context)
        {
        }
    }
}
