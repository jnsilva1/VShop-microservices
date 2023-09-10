using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VShop.ProductApi;

public class ProductDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Price is required")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [MinLength(5)]
    [MaxLength(200)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Stock is required")]
    [Range(1, 9999)]
    public long Stock { get; set; }
    public string? ImageURL { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
    public string? CategoryName { get; set; }
    public int CategoryId { get; set; }
}
