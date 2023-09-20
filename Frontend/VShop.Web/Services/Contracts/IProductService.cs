﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VShop.Web.Models;

namespace VShop.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> GetAllProducts();
    Task<ProductViewModel> FindProductById(int id);
    Task<ProductViewModel> CreateProduct(ProductViewModel productViewModel);
    Task<ProductViewModel> UpdateProduct(ProductViewModel productViewModel);
    Task<bool> DeleteProductById(int id);
}
