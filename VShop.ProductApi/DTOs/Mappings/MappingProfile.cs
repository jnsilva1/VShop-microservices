using System;
using AutoMapper;

namespace VShop.ProductApi;

public class MappingProfile : Profile
{
    protected MappingProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>().ReverseMap();
    }
}
