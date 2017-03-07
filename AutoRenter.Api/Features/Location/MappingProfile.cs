using AutoMapper;

namespace AutoRenter.Api.Features.Location
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Location, GetAll.Model.Location>()
                .ForMember(dest => dest.VehicleCount, opt => opt.MapFrom(src => src.Vehicles.Count));
            CreateMap<Domain.Location, Get.Model>()
                .ForMember(dest => dest.VehicleCount, opt => opt.MapFrom(src => src.Vehicles.Count));
            CreateMap<Domain.Location, Delete.Command>();
            CreateMap<Domain.Location, PostPut.Command>();
        }
    }
}