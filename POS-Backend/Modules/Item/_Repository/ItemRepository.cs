using Microsoft.EntityFrameworkCore;
using POS_Backend.Context;
using POS_Backend.Exceptions;
using POS_Backend.Modules.Item._Dto;
using POS_Backend.Modules.Item._Repository._Interface;

namespace POS_Backend.Modules.Item._Repository;

public class ItemRepository : IItemRepository
{
    private AppDbContext _context;
    private IConfiguration _configuration;

    public ItemRepository(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ItemResponseDto> CreateItemAsync(ItemRequestCreateDto itemRequestCreateDto)
    {
        if (itemRequestCreateDto.Image == null)
        {
            throw new BadRequestException("Image tidak boleh kosong");
        }

        try
        {
            string imageUrl = UploadImage(itemRequestCreateDto.Image);
            
            Entities.Item item = new Entities.Item
            {
                Id = Guid.NewGuid().ToString(),
                Name = itemRequestCreateDto.Name,
                Price = itemRequestCreateDto.Price,
                Stok = itemRequestCreateDto.Stok,
                PictureUrl = imageUrl,
                CategoryId = itemRequestCreateDto.CategoryId
            };

            var entityEntry = await _context.AddAsync(item);
            var entity = entityEntry.Entity;
            await _context.SaveChangesAsync();
            return new ItemResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Stok = entity.Stok,
                PictureUrl = entity.PictureUrl,
                CategoryId = entity.CategoryId
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<ItemResponseDto> GetItemIdAsync(string id)
    {
        var item = await _context.Items.Where(i => i.Id.Equals(id) && !i.IsDeleted).Include(i => i.Category).FirstOrDefaultAsync();
        if (item == null) throw new NotFoundException("Item tidak ditemukan");
        return new ItemResponseDto
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            Stok = item.Stok,
            PictureUrl = item.PictureUrl,
            CategoryId = item.CategoryId,
            Category = item.Category.Name
        };
    }

    public async Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync()
    {
        var result = await _context.Items
            .Where(i => !i.IsDeleted)
            .Include(i => i.Category)
            .ToListAsync();
        return result.Select(r => new ItemResponseDto
        {
            Id = r.Id,
            Name = r.Name,
            Price = r.Price,
            Stok = r.Stok,
            PictureUrl = r.PictureUrl,
            CategoryId = r.CategoryId,
            Category = r.Category.Name
        });
    }

    public ItemResponseDto UpdateItem(string id, ItemRequestEditDto itemRequestEditDto)
    {
        var result = GetItemById(id);
        try
        {
            if (itemRequestEditDto.Image != null)
            {
                DeleteFile(result.PictureUrl);
                var newImage = UploadImage(itemRequestEditDto.Image);
                result.PictureUrl = newImage;
            }

            result.CategoryId = itemRequestEditDto.CategoryId;
            result.Name = itemRequestEditDto.Name;
            result.Price = itemRequestEditDto.Price;
            var update = _context.Update(result);
            _context.SaveChanges();
            return new ItemResponseDto
            {
                Id = update.Entity.Id,
                Name = update.Entity.Name,
                Price = update.Entity.Price,
                Stok = update.Entity.Stok,
                PictureUrl = update.Entity.PictureUrl,
                CategoryId = update.Entity.CategoryId
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public ItemResponseDto DeleteItem(string id)
    {
        var result = GetItemById(id);
        try
        {
            result.IsDeleted = true;
            var update = _context.Update(result);
            _context.SaveChanges();
            return new ItemResponseDto
            {
                Id = update.Entity.Id,
                Name = update.Entity.Name,
                Price = update.Entity.Price,
                Stok = update.Entity.Stok,
                PictureUrl = update.Entity.PictureUrl,
                CategoryId = update.Entity.CategoryId
            };
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private Entities.Item GetItemById(string id)
    {
        var result = _context.Items.FirstOrDefault(i => i.Id.Equals(id) && !i.IsDeleted);
        if (result == null) throw new NotFoundException("Item tidak ditemukan");
        return result;
    }
    private string UploadImage(IFormFile image)
    {
        var uploadDirectory = Path.Combine(_configuration["Files"], "Images");
        if (!Directory.Exists(uploadDirectory))
        {
            Directory.CreateDirectory(uploadDirectory);
        }

        var allowedExtensions = _configuration["AllowedExtensionImages"].Split(',');
        var fileExtension = Path.GetExtension(image.FileName);
        if (!allowedExtensions.Contains(fileExtension.ToLower()))
        {
            throw new BadRequestException("Tipe file tidak diizinkan!");
        }

        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        string filePath = Path.Combine(uploadDirectory, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            image.CopyTo(stream);
        }

        return uniqueFileName;
    }

    private void DeleteFile(string fileName)
    {
        try
        {
            string fileDirectory = Path.Combine(_configuration["files"], "Images", fileName);
            if (!File.Exists(fileDirectory))
            {
                throw new BadRequestException("Gambar tidak ditemukan");
            }
            else
            {
                File.Delete(fileDirectory);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}