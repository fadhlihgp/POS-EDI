using Microsoft.EntityFrameworkCore;
using POS_Backend.Context;
using POS_Backend.Exceptions;
using POS_Backend.Modules.Category._Dto;

namespace POS_Backend.Modules.Category._Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _appDbContext;

    public CategoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<Entities.Category>> GetAllCategories()
    {
        return await _appDbContext.Categories.Where(c => !c.IsDeleted).ToListAsync();
    }

    public Entities.Category GetCategoryById(string id)
    {
        var result = _appDbContext.Categories.FirstOrDefault(c => !c.IsDeleted && c.Id.Equals(id));
        if (result == null) throw new NotFoundException("Kategori tidak ditemukan");
        return result;
    }

    public async Task<Entities.Category> CreateCategoryAsync(CategoryRequestDto categoryRequestDto)
    {
        var category = new Entities.Category
        {
            Id = Guid.NewGuid().ToString(),
            Name = categoryRequestDto.Name,
            IsDeleted = false
        };
        try
        {
            var entityEntry = await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Entities.Category UpdateCategory(string id, CategoryRequestDto categoryRequestDto)
    {
        var result = GetCategoryById(id);
        try
        {
            result.Name = categoryRequestDto.Name;
            var entityEntry = _appDbContext.Update(result);
            _appDbContext.SaveChanges();
            return entityEntry.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void DeleteCategory(string id)
    {
        var result = GetCategoryById(id);
        try
        {
            result.IsDeleted = true;
            var entityEntry = _appDbContext.Update(result);
            _appDbContext.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}