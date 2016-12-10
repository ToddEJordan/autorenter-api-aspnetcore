using AutoMapper;

namespace AutoRenter.API.Features.Vehicle
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Vehicle, GetAll.Model.Vehicle>();
            CreateMap<Domain.Vehicle, Get.QueryModel>();
            CreateMap<Domain.Vehicle, Delete.Command>();
            CreateMap<Domain.Vehicle, PostPut.Command>();
        }
    }
}