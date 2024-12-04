namespace POS_Backend.Modules.Item._Dto;

public class ItemResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stok { get; set; }
    public string PictureUrl { get; set; }
    public string CategoryId { get; set; }
    public string? Category { get; set; }
}