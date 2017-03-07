using AutoMapper;

namespace AutoRenter.Api.Features.Sku
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Sku, GetAll.Model.Sku>();
        }
    }
}