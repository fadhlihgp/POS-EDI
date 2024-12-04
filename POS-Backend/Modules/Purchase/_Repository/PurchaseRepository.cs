using Microsoft.EntityFrameworkCore;
using POS_Backend.Context;
using POS_Backend.Entities;
using POS_Backend.Exceptions;
using POS_Backend.Modules.Item._Repository._Interface;
using POS_Backend.Modules.Purchase._Dto;
using POS_Backend.Modules.Purchase._Repository._Interface;

namespace POS_Backend.Modules.Purchase._Repository;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly AppDbContext _context;

    public PurchaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PurchaseResponseDto> CreatePurchaseAsync(PurchaseRequestDto purchaseRequestDto)
    {
        List<string> itemStockNotEnough = new List<string>();
        
        ICollection<PurchaseDetail> purchaseDetails = new List<PurchaseDetail>();
        foreach (var purchaseDetailRequestDto in purchaseRequestDto.PurchaseDetails)
        {
            purchaseDetails.Add(new PurchaseDetail
            {
                Id = Guid.NewGuid().ToString(),
                Price = purchaseDetailRequestDto.Price,
                Quantity = purchaseDetailRequestDto.Quantity,
                Total = purchaseDetailRequestDto.Quantity * purchaseDetailRequestDto.Price,
                ItemId = purchaseDetailRequestDto.ItemId,
            });
        }

        try
        {
            await _context.Database.BeginTransactionAsync();
            
            // Save purchase
            Entities.Purchase savePurchase = new Entities.Purchase
            {
                Id = Guid.NewGuid().ToString(),
                Date = purchaseRequestDto.Date,
                Invoice = purchaseRequestDto.Invoice,
                PurchaseDetails = purchaseDetails
            };
            
            foreach (var savePurchasePurchaseDetail in savePurchase.PurchaseDetails)
            {
                var item = await _context.Items.FindAsync(savePurchasePurchaseDetail.ItemId);
                if (item.Stok < savePurchasePurchaseDetail.Quantity)
                {
                    itemStockNotEnough.Add(item.Name);
                }
                savePurchasePurchaseDetail.Item = item;
            }
            
            // If stok not enough more than 0
            if (itemStockNotEnough.Count() > 0)
            {
                string items = "";
                foreach (var se in itemStockNotEnough)
                {
                    items += $"{se}, ";
                }

                throw new BadRequestException(
                    "Mohon maaf, stock item "+ items + " tidak mencukupi untuk melakukan transaksi");
            }
            
            var purchaseEntity = await _context.Purchases.AddAsync(savePurchase);
            await _context.SaveChangesAsync();
            
            // Update purchase stok
            foreach (var entityPurchaseDetail in purchaseEntity.Entity.PurchaseDetails)
            {
                entityPurchaseDetail.Item.Stok -= entityPurchaseDetail.Quantity;
            }
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();

            return new PurchaseResponseDto
            {
                Id = purchaseEntity.Entity.Id,
                Date = purchaseEntity.Entity.Date,
                Invoice = purchaseEntity.Entity.Invoice,
                PurchaseDetails = purchaseEntity.Entity.PurchaseDetails.Select(pd => new PurchaseDetailResponseDto
                {
                    Id = pd.Id,
                    ItemId = pd.ItemId,
                    Item = pd.Item.Name,
                    Quantity = pd.Quantity,
                    Price = pd.Price,
                    Total = pd.Total,
                    PictureUrl = pd.Item.PictureUrl
                }),
                TotalPrice = purchaseEntity.Entity.PurchaseDetails.Sum(pd => pd.Total)
            };
        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<PurchaseResponseDto>> GetAllPurchasesAsync()
    {
        var purchases = await _context.Purchases
            .Include(p => p.PurchaseDetails).ThenInclude(pd => pd.Item).ThenInclude(i => i.Category)
            .ToListAsync();
        return purchases.Select(p => new PurchaseResponseDto
        {
            Id = p.Id,
            Date = p.Date,
            Invoice = p.Invoice,
            PurchaseDetails = p.PurchaseDetails.Select(pd => new PurchaseDetailResponseDto
            {
                Id = pd.Id,
                ItemId = pd.ItemId,
                Item = pd.Item.Name,
                Category = pd.Item.Category.Name,
                Quantity = pd.Quantity,
                Price = pd.Price,
                Total = pd.Total,
                PictureUrl = pd.Item.PictureUrl
            }),
            TotalPrice = p.PurchaseDetails.Sum(pd => pd.Total)
        });
    }

    public PurchaseResponseDto GetPurchaseDetail(string id)
    {
        var purchase = _context.Purchases
            .Where(p => p.Id.Equals(id))
            .Include(p => p.PurchaseDetails).ThenInclude(pd => pd.Item).ThenInclude(i => i.Category)
            .FirstOrDefault();
        if (purchase == null) throw new NotFoundException("Transaksi tidak ditemukan");

        return new PurchaseResponseDto
        {
            Id = purchase.Id,
            Date = purchase.Date,
            Invoice = purchase.Invoice,
            PurchaseDetails = purchase.PurchaseDetails.Select(pd => new PurchaseDetailResponseDto
            {
                Id = pd.Id,
                ItemId = pd.ItemId,
                Item = pd.Item.Name,
                Category = pd.Item.Category.Name,
                Quantity = pd.Quantity,
                Price = pd.Price,
                Total = pd.Total,
                PictureUrl = pd.Item.PictureUrl
            }),
            TotalPrice = purchase.PurchaseDetails.Sum(pd => pd.Total)
        };
    }
}