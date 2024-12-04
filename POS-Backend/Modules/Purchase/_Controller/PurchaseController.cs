using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_Backend.Modules.Purchase._Dto;
using POS_Backend.Modules.Purchase._Repository._Interface;
using POS_Backend.ViewModels;

namespace POS_Backend.Modules.Purchase._Controller;

[ApiController]
[Authorize]
[Route("api/v1/purchase")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseRepository _purchaseRepository;

    public PurchaseController(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewPurchaseAsync([FromBody] PurchaseRequestDto purchaseRequestDto)
    {
        var result = await _purchaseRepository.CreatePurchaseAsync(purchaseRequestDto);
        return Created("api/v1/items", new SingleDataResponse
        {
            Message = "Transaksi pembelian berhasil dilakukan",
            Data = result
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPurchasesAsync()
    {
        var result = await _purchaseRepository.GetAllPurchasesAsync();
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mendapatkan data transaksi pembelian",
            Data = result,
            TotalData = result.Count()
        });
    }

    [HttpGet("{id}")]
    public IActionResult GetPurchaseByIdAsync([FromRoute] string id)
    {
        var result = _purchaseRepository.GetPurchaseDetail(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mendapatkan data transaksi pembelian",
            Data = result
        });
    }
}