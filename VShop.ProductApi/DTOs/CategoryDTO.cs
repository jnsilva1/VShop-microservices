using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VShop.ProductApi;

public class CategoryDTO
{

    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Name { get; set; }

    public ICollection<Product>? Products { get; set; }
}
