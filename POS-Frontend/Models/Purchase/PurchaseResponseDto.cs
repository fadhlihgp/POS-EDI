namespace POS_Frontend.Models.Purchase;

public class PurchaseResponseDto
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public string Invoice { get; set; }
    public IEnumerable<PurchaseDetailResponseDto> PurchaseDetails { get; set; }
    public decimal TotalPrice { get; set; }
}

public class PurchaseDetailResponseDto
{
    public string Id { get; set; }
    public string ItemId { get; set; }
    public string Item { get; set; }
    public string Category { get; set; }
    public string PictureUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
}