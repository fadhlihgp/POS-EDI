using System.Drawing;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Controllers;

[Authorize]
public class ReportController : Controller
{
    private readonly IItemService _itemService;
    private readonly IPurchaseService _purchaseService;
    private INotyfService _notyfService;

    public ReportController(IItemService itemService, INotyfService notyfService, IPurchaseService purchaseService)
    {
        _itemService = itemService;
        _notyfService = notyfService;
        _purchaseService = purchaseService;
    }

    [HttpGet]
    public async Task<IActionResult> Item()
    {
        var result = await _itemService.GetAllItems();
        if (result.isSuccess)
        {
            return View(result.itemResponseVms);
        }
        _notyfService.Error(result.message);
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Purchase()
    {
        var result = await _purchaseService.GetAllPurchases();
        if (result.isSuccess)
        {
            return View(result.purchaseResponseVms);
        }
        _notyfService.Error(result.message);
        return View();
    }

    public async Task<IActionResult> ExportToExcelItem()
    {
        var items = await _itemService.GetAllItems();
        if (!items.isSuccess)
        {
            _notyfService.Error("Gagal eksport laporan item");
        }

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Item");

            // Header
            worksheet.Cells[1, 1].Value = "Item";
            worksheet.Cells[1, 2].Value = "Kategori";
            worksheet.Cells[1, 3].Value = "Stok";
            worksheet.Cells[1, 4].Value = "Harga";
            using (var range = worksheet.Cells[1, 1, 1, 5])
            {
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            
            // Data
            int row = 2;
            foreach (var item in items.itemResponseVms)
            {
                worksheet.Cells[row, 1].Value = item.Name;
                worksheet.Cells[row, 2].Value = item.Category;
                worksheet.Cells[row, 3].Value = item.Stok;
                worksheet.Cells[row, 4].Value = item.Price;
                row++;
            }

            worksheet.Cells.AutoFitColumns();
            
            // Save the Excel file
            var fileName = "ItemReport.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileContents = package.GetAsByteArray();
            return File(fileContents, contentType, fileName);
        }
    }
    
    public async Task<IActionResult> ExportToExcelPurchase()
    {
        try
        {
            var purchases = await _purchaseService.GetAllPurchases();
            if (!purchases.isSuccess) throw new Exception(purchases.message);
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Transaksi");

                // Header
                worksheet.Cells[1, 1].Value = "Invoice";
                worksheet.Cells[1, 2].Value = "Tanggal";
                worksheet.Cells[1, 3].Value = "Total Transaksi";
                using (var range = worksheet.Cells[1, 1, 1, 3])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            
                // Data
                int row = 2;
                foreach (var item in purchases.purchaseResponseVms)
                {
                    worksheet.Cells[row, 1].Value = item.Invoice;
                    worksheet.Cells[row, 2].Value = item.Date.Date;
                    worksheet.Cells[row, 2].Style.Numberformat.Format = "dd/MM/yyyy";
                    worksheet.Cells[row, 3].Value = item.TotalPrice;
                    worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";
                    row++;
                }
                worksheet.Cells.AutoFitColumns();
            
                
                var worksheet2 = package.Workbook.Worksheets.Add("Transaksi Detail");
                // Header
                worksheet2.Cells[1, 1].Value = "Invoice";
                worksheet2.Cells[1, 2].Value = "Tanggal";
                worksheet2.Cells[1, 3].Value = "Item";
                worksheet2.Cells[1, 4].Value = "Kategori";
                worksheet2.Cells[1, 5].Value = "Quantity";
                worksheet2.Cells[1, 6].Value = "Harga";
                worksheet2.Cells[1, 7].Value = "Total";
                using (var range = worksheet2.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
            
                // Data
                int row2 = 2;
                foreach (var item in purchases.purchaseResponseVms)
                {
                    foreach (var detail in item.PurchaseDetails)
                    {
                        worksheet2.Cells[row2, 1].Value = item.Invoice;
                        worksheet2.Cells[row2, 2].Value = item.Date;
                        worksheet2.Cells[row2, 2].Style.Numberformat.Format = "dd/MM/yyyy";
                        worksheet2.Cells[row2, 3].Value = detail.Item;
                        worksheet2.Cells[row2, 4].Value = detail.Category;
                        worksheet2.Cells[row2, 5].Value = detail.Quantity;
                        worksheet2.Cells[row2, 6].Value = detail.Price;
                        worksheet2.Cells[row2, 6].Style.Numberformat.Format = "#,##0.00";
                        worksheet2.Cells[row2, 7].Value = detail.Total;
                        worksheet2.Cells[row2, 7].Style.Numberformat.Format = "#,##0.00";
                        row2++;
                    }
                }
                worksheet2.Cells.AutoFitColumns();
                
                // Save the Excel file
                var fileName = "PurchaseReport.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileContents = package.GetAsByteArray();
                return File(fileContents, contentType, fileName);
            }
        }
        catch (Exception e)
        {
            _notyfService.Error("Gagal eksport laporan item");
            Console.WriteLine(e);
            throw;
        }

       
    }
}