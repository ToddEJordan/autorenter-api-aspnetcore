using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Models.Vehicle;

namespace AutoRenter.API.Features.Vehicle
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Vehicle, VehicleModel>();
            CreateMap<VehicleModel, Domain.Vehicle>().ForMember(c => c.Location, opt => opt.Ignore());
        }
    }
}
