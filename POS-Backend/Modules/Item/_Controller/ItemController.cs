using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_Backend.Modules.Item._Dto;
using POS_Backend.Modules.Item._Repository._Interface;
using POS_Backend.ViewModels;

namespace POS_Backend.Modules.Item._Controller;

[ApiController]
[Authorize]
[Route("api/v1/item")]
public class ItemController : ControllerBase
{
    private IItemRepository _itemRepository;

    public ItemController(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateItemAsync([FromForm] ItemRequestCreateDto itemRequestCreateDto)
    {
        var result = await _itemRepository.CreateItemAsync(itemRequestCreateDto);
        return Created("api/v1/item", new SingleDataResponse
        {
            Message = "Berhasil membuat item baru",
            Data = result,
        });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemByIdAsync([FromRoute]string id)
    {
        var result = await _itemRepository.GetItemIdAsync(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mendapatkan data item",
            Data = result
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetItemsAsync()
    {
        var result = await _itemRepository.GetAllItemsAsync();
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mendapatkan data item",
            Data = result,
            TotalData = result.Count()
        });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateItem([FromRoute] string id, [FromForm] ItemRequestEditDto itemRequestEditDto)
    {
        var result = _itemRepository.UpdateItem(id, itemRequestEditDto);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil memperbarui item",
            Data = result
        });
    }

    [HttpPut("delete/{id}")]
    public IActionResult DeleteItem([FromRoute] string id)
    {
        var result = _itemRepository.DeleteItem(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil menghapus data item",
            Data = result
        });
    }
}