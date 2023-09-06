using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VShop.ProductApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext context;

    public ProductRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsCategory()
    {
        return await context.Products.Include(entity => entity.Category).ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
#pragma warning disable 8603
        return await context.Products.Where(entity => entity.Id == id).FirstOrDefaultAsync();
#pragma warning restore 8603
    }

    public async Task<Product> Create(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        context.Products.Entry(product).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Delete(Product product)
    {
        return await Delete(id: product.Id);
    }

    public async Task<Product> Delete(int id)
    {
        Product product = await GetById(id);
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return product;
    }
}
