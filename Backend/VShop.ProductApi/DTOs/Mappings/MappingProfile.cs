using System;
using AutoMapper;

namespace VShop.ProductApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();

        CreateMap<ProductDTO, Product>();

        CreateMap<Product, ProductDTO>()
        .ForMember(
            destinationMember: productDTO => productDTO.CategoryName,
            memberOptions: options => options.MapFrom(productEntity => productEntity.Category == null ? null : productEntity.Category.Name)
        );
    }
}
