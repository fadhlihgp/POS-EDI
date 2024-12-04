using POS_Frontend.Models;
using POS_Frontend.Models.Category;

namespace POS_Frontend.Services.Interfaces;

public interface ICategoryService
{
    public Task<(bool isSuccess, string message, IEnumerable<CategoryResponseVm>? categoryResponseVms)> GetAllCategories();
    public Task<(bool isSuccess, string message, CategoryResponseVm? categoryResponseVm)> GetCategoryById(string id);
    public Task<ResponseVm?> CreateCategoryAsync(CategoryRequestVm categoryRequestDto);
    public Task<ResponseVm?> UpdateCategory(string id, CategoryRequestVm categoryRequestDto);
    public Task<ResponseVm?> DeleteCategory(string id);
}