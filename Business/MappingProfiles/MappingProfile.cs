
using AutoMapper;
using Business.Dtos;
using Infrastructure.Entities;

namespace Business.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerEntity, Customer>();
            CreateMap<ProductVariantEntity, ProductVariant>();
            CreateMap<CustomerOrderEntity, CustomerOrder>();

        }
    }
}
