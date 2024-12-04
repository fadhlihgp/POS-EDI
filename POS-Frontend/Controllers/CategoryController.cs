using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_Frontend.Models.Category;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private INotyfService _notyfService;
    
    public CategoryController(ICategoryService categoryService, INotyfService notyfService)
    {
        _categoryService = categoryService;
        _notyfService = notyfService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var allCategories = await _categoryService.GetAllCategories();
        if (allCategories.isSuccess)
        {
            return View(allCategories.categoryResponseVms);
        }
        else
        {
            _notyfService.Error(allCategories.message);
            return View();
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        CategoryRequestVm categoryRequestVm = new CategoryRequestVm();
        return View(categoryRequestVm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryRequestVm categoryRequestVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.CreateCategoryAsync(categoryRequestVm);
            if (!result.IsSuccess)
            {
                _notyfService.Error(result.Message);
                return View(categoryRequestVm);
            }
            _notyfService.Success(result.Message);
            return RedirectToAction("Index");
        }

        return View(categoryRequestVm);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var result = await _categoryService.GetCategoryById(id);
        if (!result.isSuccess)
        {
            _notyfService.Error(result.message);
            return RedirectToAction("Index");
        }

        CategoryRequestVm categoryRequestVm = new CategoryRequestVm
        {
            Name = result.categoryResponseVm.Name
        };
        ViewBag.Id = result.categoryResponseVm.Id;
        return View(categoryRequestVm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, CategoryRequestVm categoryRequestVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.UpdateCategory(id, categoryRequestVm);
            if (!result.IsSuccess)
            {
                _notyfService.Error(result.Message);
                return View(categoryRequestVm);
            }
            _notyfService.Success(result.Message);
            return RedirectToAction("Index");
        }

        return View(categoryRequestVm);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _categoryService.DeleteCategory(id);
        if (!result.IsSuccess)
        {
            _notyfService.Error(result.Message);
            return RedirectToAction("Index");
        }
        _notyfService.Success(result.Message);
        return RedirectToAction("Index");
    }
}