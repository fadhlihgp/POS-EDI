using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_Backend.Modules.Category._Dto;
using POS_Backend.Modules.Category._Repository;
using POS_Backend.ViewModels;

namespace POS_Backend.Modules.Category._Controller;

[ApiController]
[Authorize]
[Route("api/v1/category")]
public class CategoryController : ControllerBase
{
    private ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryRepository.GetAllCategories();
        return Ok(new MultipleDataResponse
        {
            Message = "Berhasil mendapatkan data kategori",
            Data = result,
            TotalData = result.Count()
        });
    }

    [HttpGet("{id}")]
    public IActionResult GetCategoryById([FromRoute] string id)
    {
        var result = _categoryRepository.GetCategoryById(id);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil mendapatkan data kategori",
            Data = result
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryRequestDto categoryRequestDto)
    {
        var result = await _categoryRepository.CreateCategoryAsync(categoryRequestDto);
        return Created("api/v1/category", new SingleDataResponse
        {
            Message = "Berhasil membuat data kategori",
            Data = result
        });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategory([FromRoute] string id, [FromBody] CategoryRequestDto categoryRequestDto)
    {
        var result = _categoryRepository.UpdateCategory(id, categoryRequestDto);
        return Ok(new SingleDataResponse
        {
            Message = "Berhasil memperbarui kategori",
            Data = result
        });
    }
    
    [HttpPut("delete/{id}")]
    public IActionResult DeleteCategory([FromRoute] string id)
    {
        _categoryRepository.DeleteCategory(id);
        return Ok(new NoDataResponse()
        {
            Message = "Berhasil menghapus kategori"
        });
    }
}