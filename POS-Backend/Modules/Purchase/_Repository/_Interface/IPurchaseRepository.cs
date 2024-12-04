using POS_Backend.Modules.Purchase._Dto;

namespace POS_Backend.Modules.Purchase._Repository._Interface;

public interface IPurchaseRepository
{
    public Task<PurchaseResponseDto> CreatePurchaseAsync(PurchaseRequestDto purchaseRequestDto);
    public Task<IEnumerable<PurchaseResponseDto>> GetAllPurchasesAsync();
    public PurchaseResponseDto GetPurchaseDetail(string id);
}