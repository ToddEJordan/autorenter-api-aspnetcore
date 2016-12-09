using AutoMapper;
using AutoRenter.API.Models.Location;

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