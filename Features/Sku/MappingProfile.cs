using AutoMapper;

namespace AutoRenter.API.Features.Sku
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Sku, GetAll.Model.Sku>();
        }
    }
}