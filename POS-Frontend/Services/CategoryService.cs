using Newtonsoft.Json;
using POS_Frontend.Helpers;
using POS_Frontend.Models;
using POS_Frontend.Models.Category;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Services;

public class CategoryService : ICategoryService
{
    private IBaseService _baseService;
    private string _categoryUrl;

    public CategoryService( IBaseService baseService)
    {
        _baseService = baseService;
        _categoryUrl = "/api/v1/category";
    }

    public async Task<(bool isSuccess, string message, IEnumerable<CategoryResponseVm>? categoryResponseVms)> GetAllCategories()
    {
        var responseVm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.GET,
            Url = _categoryUrl,
            Data = null
        }, true);
        return (responseVm.IsSuccess, responseVm.Message,
            JsonConvert.DeserializeObject<IEnumerable<CategoryResponseVm>>(Convert.ToString(responseVm.Data)));
    }

    public async Task<(bool isSuccess, string message, CategoryResponseVm? categoryResponseVm)> GetCategoryById(string id)
    {
        var responsevm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.GET,
            Url = $"{_categoryUrl}/{id}",
            Data = null
        }, true);
        return (responsevm.IsSuccess, responsevm.Message,
            JsonConvert.DeserializeObject<CategoryResponseVm>(Convert.ToString(responsevm.Data)));
    }

    public async Task<ResponseVm?> CreateCategoryAsync(CategoryRequestVm categoryRequestDto)
    {
        return await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.POST,
            Url = _categoryUrl,
            Data = categoryRequestDto
        }, true);
    }

    public async Task<ResponseVm?> UpdateCategory(string id, CategoryRequestVm categoryRequestDto)
    {
        return await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.PUT,
            Url = $"{_categoryUrl}/{id}",
            Data = categoryRequestDto
        }, true);
    }

    public async Task<ResponseVm?> DeleteCategory(string id)
    {
        return await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.PUT,
            Url = $"{_categoryUrl}/delete/{id}",
            Data = null
        }, true);
    }
}