using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VShop.ProductApi.Roles;
using VShop.ProductApi.Services;

namespace VShop.ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService categoryService)
    {
        _productService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
    {
        var productsDto = await _productService.GetProducts();
        if (productsDto is null)
            return NotFound(value: "Products not found");

        return Ok(value: productsDto);
    }

    [HttpGet(template: "{id}", Name = "GetProduct")]
    public async Task<ActionResult<ProductDTO>> Get(int id)
    {
        var productDto = await _productService.GetProductById(id: id);
        if (productDto is null)
            return NotFound(value: "Product not found");

        return Ok(value: productDto);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ProductDTO productDTO)
    {
        if (productDTO is null) return BadRequest(error: "Invalid Data");

        await _productService.AddProduct(productDTO: productDTO);

        return new CreatedAtRouteResult(
            routeName: "GetProduct",
            routeValues: new { id = productDTO.Id },
            value: productDTO
        );
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromBody] ProductDTO productDTO)
    {
        if (productDTO is null) return BadRequest();

        await _productService.UpdateProduct(productDTO: productDTO);

        return Ok(value: productDTO);
    }

    [HttpDelete(template: "{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult<ProductDTO>> Delete(int id)
    {
        ProductDTO productDTO = await _productService.GetProductById(id: id);
        if (productDTO is null) return NotFound(value: "Product not found");

        await _productService.DeleteProduct(id: productDTO.Id);

        return Ok(value: productDTO);
    }
}
