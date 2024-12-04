using System.ComponentModel.DataAnnotations;

namespace POS_Frontend.Models.Category;

public class CategoryRequestVm
{
    [Required(ErrorMessage = "Nama Kategori tidak boleh kosong")]
    public string Name { get; set; }
}

public class CategoryResponseVm
{
    public string Id { get; set; }
    public string Name { get; set; }
}