
using POS_Backend.Modules.Category._Dto;

namespace POS_Backend.Modules.Category._Repository;

public interface ICategoryRepository
{
    public Task<IEnumerable<Entities.Category>> GetAllCategories();
    public Entities.Category GetCategoryById(string id);
    public Task<Entities.Category> CreateCategoryAsync(CategoryRequestDto categoryRequestDto);
    public Entities.Category UpdateCategory(string id, CategoryRequestDto categoryRequestDto);
    public void DeleteCategory(string id);
}