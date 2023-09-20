using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using VShop.ProductApi.Repositories;

namespace VShop.ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        this._productRepository = productRepository;
        this._mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> GetProducts()
    {
        IEnumerable<Product> productsEntity = await _productRepository.GetAll();
        return _mapper.Map<IEnumerable<ProductDTO>>(source: productsEntity);
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsCategories()
    {
        IEnumerable<Product> productsEntity = await _productRepository.GetProductsCategory();
        return _mapper.Map<IEnumerable<ProductDTO>>(source: productsEntity);
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        Product productEntity = await _productRepository.GetById(id: id);
        return _mapper.Map<ProductDTO>(source: productEntity);
    }

    public async Task AddProduct(ProductDTO productDTO)
    {
        Product product = _mapper.Map<Product>(source: productDTO);
        await _productRepository.Create(product: product);
        productDTO.Id = product.Id;
    }

    public async Task UpdateProduct(ProductDTO productDTO)
    {
        Product product = _mapper.Map<Product>(source: productDTO);
        await _productRepository.Update(product: product);
    }

    public async Task DeleteProduct(int id)
    {
        Product productEntity = await _productRepository.GetById(id: id);
        await _productRepository.Delete(id: productEntity.Id);
    }
}
