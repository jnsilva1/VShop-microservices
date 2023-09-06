using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VShop.ProductApi.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<IEnumerable<Product>> GetProductsCategory();
    Task<Product> GetById(int id);
    Task<Product> Create(Product product);
    Task<Product> Update(Product product);
    Task<Product> Delete(Product product);
    Task<Product> Delete(int id);
}
