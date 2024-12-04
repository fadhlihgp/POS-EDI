using POS_Frontend.Models.Item;

namespace POS_Frontend.Services.Interfaces;

public interface IItemService
{
    public Task<(bool isSuccess, string message, IEnumerable<ItemResponseDto>? itemResponseVms)> GetAllItems();
    public Task<(bool isSuccess, string message, ItemResponseDto? itemResponseVm)> GetItemById(string id);
    public Task<(bool isSuccess, string message, ItemResponseDto? itemResponseDto)> CreateNewItem(
        ItemRequestCreateDto itemRequestCreateDto);
    public Task<(bool isSuccess, string message, ItemResponseDto? itemResponseDto)> EditItem(
        ItemRequestEditDto itemRequestEditDto);
    public Task<(bool isSuccess, string message, ItemResponseDto? itemResponseDto)> DeleteItem(string id);
}