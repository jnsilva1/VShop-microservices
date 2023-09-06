using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VShop.ProductApi.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext context;

    public CategoryRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await context.Categories.ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetCategoriesProducts()
    {
        return await context.Categories.Include(entity => entity.Products).ToListAsync();
    }

    public async Task<Category> GetById(int id)
    {
#pragma warning disable 8603
        return await context.Categories.Where(entity => entity.CategoryId == id).FirstOrDefaultAsync();
#pragma warning restore 8603
    }

    public async Task<Category> Create(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Update(Category category)
    {
        context.Entry(category).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Delete(Category category)
    {
        return await Delete(id: category.CategoryId);
    }

    public async Task<Category> Delete(int id)
    {
        Category category = await GetById(id);
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return category;
    }
}
