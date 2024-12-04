using Newtonsoft.Json;
using POS_Frontend.Helpers;
using POS_Frontend.Models;
using POS_Frontend.Models.Item;
using POS_Frontend.Models.Purchase;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IBaseService _baseService;
    private string _purchaseUrl;

    public PurchaseService(IBaseService baseService)
    {
        _baseService = baseService;
        _purchaseUrl = "/api/v1/purchase";
    }

    public async Task<(bool isSuccess, string message, PurchaseResponseDto? purchaseResponseDto)> CreatePurchase(PurchaseRequestDto purchaseRequestDto)
    {
        var responseVm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.POST,
            Url = _purchaseUrl,
            Data = purchaseRequestDto
        }, true);
        return (responseVm.IsSuccess, responseVm.Message,
            JsonConvert.DeserializeObject<PurchaseResponseDto>(Convert.ToString(responseVm.Data)));
    }

    public async Task<(bool isSuccess, string message, PurchaseResponseDto? purchaseResponseVms)> GetPurchaseById(string id)
    {
        var responseVm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.GET,
            Url = $"{_purchaseUrl}/{id}",
            Data = null
        }, true);
        return (responseVm.IsSuccess, responseVm.Message,
            JsonConvert.DeserializeObject<PurchaseResponseDto>(Convert.ToString(responseVm.Data)));
    }

    public async Task<(bool isSuccess, string message, IEnumerable<PurchaseResponseDto>? purchaseResponseVms)> GetAllPurchases()
    {
        var responseVm = await _baseService.SendAsync(new RequestVm
        {
            ApiType = StaticData.ApiType.GET,
            Url = _purchaseUrl,
            Data = null
        }, true);
        return (responseVm.IsSuccess, responseVm.Message,
            JsonConvert.DeserializeObject<IEnumerable<PurchaseResponseDto>>(Convert.ToString(responseVm.Data)));
    }
}