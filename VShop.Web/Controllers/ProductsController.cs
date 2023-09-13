using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService productService;
    private readonly ICategoryService categoryService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        this.productService = productService;
        this.categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await productService.GetAllProducts();
        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        ViewBag.CategoryId = new SelectList(await categoryService.GetAllCategories(), "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await productService.CreateProduct(productViewModel: productViewModel);
            if (result != null)
                return RedirectToAction(nameof(Index));
        }

        ViewBag.CategoryId = new SelectList(await categoryService.GetAllCategories(), "CategoryId", "Name");
        return View(productViewModel);
    }

}
