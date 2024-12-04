using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_Frontend.Models.Purchase;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Controllers;

[Authorize]
public class PurchaseController : Controller
{
    private readonly IPurchaseService _purchaseService;
    private readonly IItemService _itemService;
    private readonly INotyfService _notyfService;

    public PurchaseController(IPurchaseService purchaseService, INotyfService notyfService, IItemService itemService)
    {
        _purchaseService = purchaseService;
        _notyfService = notyfService;
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _purchaseService.GetAllPurchases();
        if (result.isSuccess)
        {
            return View(result.purchaseResponseVms);
        }
        else
        {
            _notyfService.Error(result.message);
            return View();
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        var items = _itemService.GetAllItems().Result.itemResponseVms;
        ViewBag.Items = items;
        PurchaseRequestDto purchaseRequestDto = new PurchaseRequestDto
        {
            Invoice = $"INV-{DateTime.Now.ToString("ddMMyymmss")}"
        };
        purchaseRequestDto.PurchaseDetails = new List<PurchaseDetailRequestDto>();
        return View(purchaseRequestDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PurchaseRequestDto purchaseRequestDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _purchaseService.CreatePurchase(purchaseRequestDto);
            if (result.isSuccess)
            {
                _notyfService.Success(result.message);
                return RedirectToAction("Index");
            }
            var itemss = _itemService.GetAllItems().Result.itemResponseVms;
            ViewBag.Items = itemss;
            _notyfService.Error(result.message, 10);
            return View(purchaseRequestDto);
        }
        purchaseRequestDto.PurchaseDetails = new List<PurchaseDetailRequestDto>();
        var items = _itemService.GetAllItems().Result.itemResponseVms;
        ViewBag.Items = items;
        return View(purchaseRequestDto);
    }
    
    [HttpGet]
    public async Task<IActionResult> Detail(string id)
    {
        var result = await _purchaseService.GetPurchaseById(id);
        if (result.isSuccess)
        {
            return View(result.purchaseResponseVms);
        }
        else
        {
            _notyfService.Error(result.message);
            return NotFound();
        }
    }
}