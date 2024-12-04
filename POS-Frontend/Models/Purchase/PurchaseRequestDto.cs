using System.ComponentModel.DataAnnotations;

namespace POS_Frontend.Models.Purchase;

public class PurchaseRequestDto
{
    public string Invoice { get; set; }
    public DateTime Date { get; set; }
    
    [MinLength(length:1, ErrorMessage = "Purchase details is required")]
    public IEnumerable<PurchaseDetailRequestDto> PurchaseDetails { get; set; }
}

public class PurchaseDetailRequestDto
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Item is required")]
    public string ItemId { get; set; }
}