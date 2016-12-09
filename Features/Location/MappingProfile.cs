using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Models.Location;
using AutoRenter.API.Models.Locations;

namespace AutoRenter.API.Features.Location
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Location, LocationModel>();
            CreateMap<LocationModel, Domain.Location>();
        }
    }
}
