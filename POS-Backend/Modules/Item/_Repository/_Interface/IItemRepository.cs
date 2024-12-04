using POS_Backend.Modules.Item._Dto;

namespace POS_Backend.Modules.Item._Repository._Interface;

public interface IItemRepository
{
    Task<ItemResponseDto> CreateItemAsync(ItemRequestCreateDto itemRequestCreateDto);
    Task<ItemResponseDto> GetItemIdAsync(string id);
    Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync();
    ItemResponseDto UpdateItem(string id, ItemRequestEditDto itemRequestEditDto);
    ItemResponseDto DeleteItem(string id);
}