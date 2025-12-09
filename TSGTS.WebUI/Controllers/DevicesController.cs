using Microsoft.AspNetCore.Mvc;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;

namespace TSGTS.WebUI.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
public class DevicesController : Controller
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    public async Task<IActionResult> Index()
    {
        var devices = await _deviceService.GetAllAsync();
        return View(devices);
    }

    public IActionResult Create()
    {
        return View(new DeviceCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DeviceCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _deviceService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var device = await _deviceService.GetByIdAsync(id);
        if (device is null)
            return NotFound();

        var editDto = new DeviceCreateDto
        {
            SerialNumber = device.SerialNumber,
            BrandId = device.BrandId,
            ModelId = device.ModelId,
            PurchaseDate = device.PurchaseDate,
            WarrantyEndDate = device.WarrantyEndDate
        };
        ViewBag.DeviceId = id;
        return View(editDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DeviceCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.DeviceId = id;
            return View(dto);
        }

        var updated = await _deviceService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _deviceService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
