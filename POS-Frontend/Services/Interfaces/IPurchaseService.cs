using POS_Frontend.Models.Purchase;

namespace POS_Frontend.Services.Interfaces;

public interface IPurchaseService
{
    public Task<(bool isSuccess, string message, PurchaseResponseDto? purchaseResponseDto)> CreatePurchase(PurchaseRequestDto purchaseRequestDto);
    public Task<(bool isSuccess, string message, PurchaseResponseDto? purchaseResponseVms)> GetPurchaseById(string id);
    public Task<(bool isSuccess, string message, IEnumerable<PurchaseResponseDto>? purchaseResponseVms)> GetAllPurchases();
}