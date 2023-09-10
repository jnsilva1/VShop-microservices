using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VShop.ProductApi.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProducts();
    Task<IEnumerable<ProductDTO>> GetProductsCategories();
    Task<ProductDTO> GetProductById(int id);
    Task AddProduct(ProductDTO productDTO);
    Task UpdateProduct(ProductDTO productDTO);
    Task DeleteProduct(int id);
}
