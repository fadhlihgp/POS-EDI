using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_Frontend.Models.Item;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Controllers;

[Authorize]
public class ItemController : Controller
{
    private readonly IItemService _itemService;
    private readonly ICategoryService _categoryService;
    private INotyfService _notyfService;
    private IConfiguration _configuration;

    public ItemController(IItemService itemService, INotyfService notyfService, IConfiguration configuration, ICategoryService categoryService)
    {
        _itemService = itemService;
        _notyfService = notyfService;
        _configuration = configuration;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var valueTuple = await _itemService.GetAllItems();
        if (valueTuple.isSuccess)
        {
            ViewBag.BasePhotoUrl = _configuration["Files"];
            return View(valueTuple.itemResponseVms);
        }
        else
        {
            _notyfService.Error(valueTuple.message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _categoryService.GetAllCategories();
        if (categories.isSuccess)
        {
            ViewBag.Categories = categories.categoryResponseVms;
            return View();
        }
        else
        {
            _notyfService.Error(categories.message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var valueTuple = await _itemService.GetItemById(id);
        var categories = await _categoryService.GetAllCategories();
        ItemRequestEditDto itemRequestEditDto = new ItemRequestEditDto();
        
        if (valueTuple.isSuccess && categories.isSuccess)
        {
            ViewBag.BasePhotoUrl = _configuration["Files"];
            ViewBag.Categories = categories.categoryResponseVms;
            ViewBag.PictureUrl = valueTuple.itemResponseVm.PictureUrl;
            
            ViewBag.Stok = valueTuple.itemResponseVm.Stok;
            
            itemRequestEditDto.Price = valueTuple.itemResponseVm.Price;
            itemRequestEditDto.Name = valueTuple.itemResponseVm.Name;
            itemRequestEditDto.CategoryId = valueTuple.itemResponseVm.CategoryId;
            itemRequestEditDto.Id = valueTuple.itemResponseVm.Id;
            
            return View(itemRequestEditDto);
        }
        else
        {
            _notyfService.Error(valueTuple.message);
            return View(itemRequestEditDto);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(ItemRequestCreateDto itemRequestCreateDto)
    {
        if (ModelState.IsValid)
        {
            var valueTuple = await _itemService.CreateNewItem(itemRequestCreateDto);
            if (valueTuple.isSuccess)
            {
                _notyfService.Success(valueTuple.message);
                return RedirectToAction("Index");
            }
            else
            {
                _notyfService.Error(valueTuple.message);
                return View(itemRequestCreateDto);
            }
        }
        var categories = await _categoryService.GetAllCategories();
        if (categories.isSuccess)
        {
            ViewBag.Categories = categories.categoryResponseVms;
        }
        return View(itemRequestCreateDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(ItemRequestEditDto itemRequestEdit)
    {
        if (ModelState.IsValid)
        {
            var valueTuple = await _itemService.EditItem(itemRequestEdit);
            if (valueTuple.isSuccess)
            {
                _notyfService.Success(valueTuple.message);
                return RedirectToAction("Index");
            }
            else
            {
                _notyfService.Error(valueTuple.message);
                return View(itemRequestEdit);
            }
        }
        var categories = await _categoryService.GetAllCategories();
        if (categories.isSuccess)
        {
            ViewBag.Categories = categories.categoryResponseVms;
        }
        return View(itemRequestEdit);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var valueTuple = await _itemService.DeleteItem(id);
        if (valueTuple.isSuccess)
        {
            _notyfService.Success(valueTuple.message);
            return RedirectToAction("Index");
        }
        else
        {
            _notyfService.Error(valueTuple.message);
            return RedirectToAction("Index");
        }
    }
}