using System;
using System.Collections.Generic;
namespace VShop.ProductApi;

public class Category
{
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public ICollection<Product>? Products { get; set; }
}
