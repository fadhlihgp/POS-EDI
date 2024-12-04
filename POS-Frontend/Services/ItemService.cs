using Newtonsoft.Json;
using POS_Frontend.Helpers;
using POS_Frontend.Models;
using POS_Frontend.Models.Category;
using POS_Frontend.Models.Item;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Services;

public class ItemService : IItemService
{
    private readonly IBaseService _baseService;
    private string _itemUrl;

    public ItemService(IBaseService baseService)
    {
        _baseService = baseService;
        _itemUrl = "/api/v1/item";
    }

    public async Task<(bool isSuccess, string message, IEnumerable<ItemResponseDto>? itemResponseVms)> GetAllItems()
    {
        var responseVm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.GET,
            Url = _itemUrl,
            Data = null
        }, true);
        return (responseVm.IsSuccess, responseVm.Message,
            JsonConvert.DeserializeObject<IEnumerable<ItemResponseDto>>(Convert.ToString(responseVm.Data)));
    }

    public async Task<(bool isSuccess, string message, ItemResponseDto? itemResponseVm)> GetItemById(string id)
    {
        var responseVm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.GET,
            Url = $"{_itemUrl}/{id}",
            Data = null
        }, true);
        return (responseVm.IsSuccess, responseVm.Message,
            JsonConvert.DeserializeObject<ItemResponseDto>(Convert.ToString(responseVm.Data)));
    }

    public async Task<(bool isSuccess, string message, ItemResponseDto? itemResponseDto)> CreateNewItem(ItemRequestCreateDto itemRequestCreateDto)
    {
        var sendData = new Dictionary<string, object>()
        {
            { "Name", itemRequestCreateDto.Name },
            { "Price", itemRequestCreateDto.Price },
            { "Stok", itemRequestCreateDto.Stok },
            { "CategoryId", itemRequestCreateDto.CategoryId },
            { "Image", itemRequestCreateDto.Image }
        };
        
        var responseVm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.POST,
            Url = $"{_itemUrl}",
            Data = sendData,
            IsFormMultipart = true
        }, true);
        return (responseVm.IsSuccess, responseVm.Message,
            JsonConvert.DeserializeObject<ItemResponseDto>(Convert.ToString(responseVm.Data)));
    }

    public async Task<(bool isSuccess, string message, ItemResponseDto? itemResponseDto)> EditItem(ItemRequestEditDto itemRequestEditDto)
    {
        var sendData = new Dictionary<string, object>()
        {
            { "Name", itemRequestEditDto.Name },
            { "Price", itemRequestEditDto.Price },
            { "CategoryId", itemRequestEditDto.CategoryId }
        };
        if (itemRequestEditDto.Image != null)
        {
            sendData.Add("Image", itemRequestEditDto.Image);
        }
        
        var responseVm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.PUT,
            Url = $"{_itemUrl}/{itemRequestEditDto.Id}",
            Data = sendData,
            IsFormMultipart = true
        }, true);
        return (responseVm.IsSuccess, responseVm.Message,
            JsonConvert.DeserializeObject<ItemResponseDto>(Convert.ToString(responseVm.Data)));
    }

    public async Task<(bool isSuccess, string message, ItemResponseDto? itemResponseDto)> DeleteItem(string id)
    {
        var responseVm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.PUT,
            Url = $"{_itemUrl}/delete/{id}",
            Data = null,
            IsFormMultipart = false
        }, true);
        return (responseVm.IsSuccess, responseVm.Message,
            JsonConvert.DeserializeObject<ItemResponseDto>(Convert.ToString(responseVm.Data)));
    }
}