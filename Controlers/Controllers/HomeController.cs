using System.Diagnostics;
using System.Web;
using Common.DTO.Message;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Services.Adapters;
using Services.Communication;
using Services.Storage;

namespace Controlers.Controllers;

public class HomeController : Controller
{
    private readonly IBlobStorageService _blobStorageService;
    
    public HomeController(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var blobs = await _blobStorageService.GetAllBlobsAsync();
        ViewBag.Blobs = blobs;
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            if (!file.ContentType.StartsWith("image"))
            {
                TempData["Message"] = "File is not image";
            }
            else
            {
                var blob = await _blobStorageService.UploadAsync(file);
            }
        }
        
        return RedirectToAction(nameof(Index));
    }
}