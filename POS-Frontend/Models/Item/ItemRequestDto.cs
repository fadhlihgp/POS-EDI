using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace POS_Frontend.Models.Item;

public class ItemRequestCreateDto
{
    [MinLength(1, ErrorMessage = "Name is required")]
    public string Name { get; set; }
    
    [Range(1.00, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Stok must be greater than 0")]
    public int Stok { get; set; }
    
    [Required]
    public IFormFile Image { get; set; }
    
    [Required(ErrorMessage = "Category is required")]
    public string CategoryId { get; set; }
}

public class ItemRequestEditDto
{
    public string Id { get; set; }
    [MinLength(1, ErrorMessage = "Name is required")]
    public string Name { get; set; }
    
    [Range(1.00, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    
    public IFormFile? Image { get; set; }
    
    [Required(ErrorMessage = "Category is required")]
    public string CategoryId { get; set; }
}