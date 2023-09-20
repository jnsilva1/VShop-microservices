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
        ViewBag.CategoryId = new SelectList(items: await categoryService.GetAllCategories(), dataValueField: "CategoryId", dataTextField: "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await productService.CreateProduct(productViewModel: productViewModel);
            if (result != null)
                return RedirectToAction(actionName: nameof(Index));
        }

        ViewBag.CategoryId = new SelectList(items: await categoryService.GetAllCategories(), dataValueField: "CategoryId", dataTextField: "Name");
        return View(productViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProduct(int id)
    {
        ViewBag.CategoryId = new SelectList(items: await categoryService.GetAllCategories(), dataValueField: "CategoryId", dataTextField: "Name");

        var result = await productService.FindProductById(id: id);
        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(ProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await productService.UpdateProduct(productViewModel: productViewModel);
            if (result is not null)
                return RedirectToAction(actionName: nameof(Index));
        }

        ViewBag.CategoryId = new SelectList(items: await categoryService.GetAllCategories(), dataValueField: "CategoryId", dataTextField: "Name");
        return View(productViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await productService.FindProductById(id: id);
        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpPost(), ActionName(name: "DeleteProduct")]
    public async Task<IActionResult> DeleteConfirmedProduct(int id)
    {
        var result = await productService.DeleteProductById(id: id);
        if (!result)
            return View("Error");

        return RedirectToAction(actionName: nameof(Index));
    }
}
