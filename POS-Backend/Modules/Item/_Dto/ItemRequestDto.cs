namespace POS_Backend.Modules.Item._Dto;

public class ItemRequestCreateDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stok { get; set; }
    public IFormFile Image { get; set; }
    public string CategoryId { get; set; }
}

public class ItemRequestEditDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public IFormFile? Image { get; set; }
    public string CategoryId { get; set; }
}