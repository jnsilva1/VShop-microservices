using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VShop.ProductApi.Services;

namespace VShop.ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{

    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
    {
        var categoriesDto = await _categoryService.GetCategories();
        if (categoriesDto is null)
            return NotFound(value: "Categories not found");

        return Ok(value: categoriesDto);
    }

    [HttpGet(template: "products")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesProduct()
    {
        var categoriesDto = await _categoryService.GetCategoriesProducts();
        if (categoriesDto is null)
            return NotFound(value: "Categories not found");

        return Ok(value: categoriesDto);
    }

    [HttpGet(template: "{id:int}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDTO>> Get(int id)
    {
        var categoryDto = await _categoryService.GetCategoryById(id: id);
        if (categoryDto is null)
            return NotFound(value: "Category not found");

        return Ok(value: categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDTO)
    {
        if (categoryDTO is null) return BadRequest(error: "Invalid Data");

        await _categoryService.AddCategory(categoryDTO: categoryDTO);

        return new CreatedAtRouteResult(
            routeName: "GetCategory",
            routeValues: new { id = categoryDTO.CategoryId },
            value: categoryDTO
        );
    }

    [HttpPut(template: "{id: int}")]
    public async Task<ActionResult> Put(int id, [FromBody] CategoryDTO categoryDTO)
    {
        if (categoryDTO is null || id != categoryDTO.CategoryId) return BadRequest();

        await _categoryService.UpdateCategory(categoryDTO: categoryDTO);

        return Ok(value: categoryDTO);
    }

    [HttpDelete(template: "{id: int}")]
    public async Task<ActionResult<CategoryDTO>> Delete(int id)
    {
        CategoryDTO categoryDTO = await _categoryService.GetCategoryById(id: id);
        if (categoryDTO is null) return NotFound(value: "Category not found");

        await _categoryService.DeleteCategory(id: categoryDTO.CategoryId);

        return Ok(value: categoryDTO);
    }
}
