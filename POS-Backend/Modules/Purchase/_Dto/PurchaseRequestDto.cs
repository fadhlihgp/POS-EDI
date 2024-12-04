namespace POS_Backend.Modules.Purchase._Dto;

public class PurchaseRequestDto
{
    public string Invoice { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<PurchaseDetailRequestDto> PurchaseDetails { get; set; }
}

public class PurchaseDetailRequestDto
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ItemId { get; set; }
}