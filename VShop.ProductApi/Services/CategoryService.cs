using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using VShop.ProductApi.Repositories;

namespace VShop.ProductApi.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        this._categoryRepository = categoryRepository;
        this._mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategories()
    {
        IEnumerable<Category> categoriesEntity = await _categoryRepository.GetAll();
        return _mapper.Map<IEnumerable<CategoryDTO>>(source: categoriesEntity);
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesProducts()
    {
        IEnumerable<Category> categoriesEntity = await _categoryRepository.GetCategoriesProducts();
        return _mapper.Map<IEnumerable<CategoryDTO>>(source: categoriesEntity);
    }

    public async Task<CategoryDTO> GetCategoryById(int id)
    {
        Category category = await _categoryRepository.GetById(id: id);
        return _mapper.Map<CategoryDTO>(category);
    }

    public async Task AddCategory(CategoryDTO categoryDTO)
    {
        Category category = _mapper.Map<Category>(source: categoryDTO);
        await _categoryRepository.Create(category: category);
        categoryDTO.CategoryId = category.CategoryId;
    }

    public async Task UpdateCategory(CategoryDTO categoryDTO)
    {
        Category category = _mapper.Map<Category>(source: categoryDTO);
        await _categoryRepository.Update(category: category);
    }

    public async Task DeleteCategory(int id)
    {
        Category categoryEntity = await _categoryRepository.GetById(id: id);
        await _categoryRepository.Delete(id: categoryEntity.CategoryId);
    }
}
