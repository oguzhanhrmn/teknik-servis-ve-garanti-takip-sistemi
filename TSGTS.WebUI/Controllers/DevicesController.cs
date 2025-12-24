using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;
using TSGTS.DataAccess.Repositories;
using TSGTS.Core.Entities;

namespace TSGTS.WebUI.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
public class DevicesController : Controller
{
    private readonly IDeviceService _deviceService;
    private readonly IGenericRepository<Brand> _brandRepository;
    private readonly IGenericRepository<Model> _modelRepository;

    public DevicesController(IDeviceService deviceService,
        IGenericRepository<Brand> brandRepository,
        IGenericRepository<Model> modelRepository)
    {
        _deviceService = deviceService;
        _brandRepository = brandRepository;
        _modelRepository = modelRepository;
    }

    public async Task<IActionResult> Index()
    {
        var devices = await _deviceService.GetAllAsync();
        return View(devices);
    }

    public IActionResult Create()
    {
        ViewBag.Brands = BuildBrandSelectList();
        ViewBag.Models = BuildModelSelectList();
        return View(new DeviceCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DeviceCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Brands = BuildBrandSelectList();
            ViewBag.Models = BuildModelSelectList();
            return View(dto);
        }

        await _deviceService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var device = await _deviceService.GetByIdAsync(id);
        if (device is null)
            return NotFound();

        ViewBag.Brands = BuildBrandSelectList(device.BrandId);
        ViewBag.Models = BuildModelSelectList(device.ModelId);
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
            ViewBag.Brands = BuildBrandSelectList(dto.BrandId);
            ViewBag.Models = BuildModelSelectList(dto.ModelId);
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

    private List<SelectListItem> BuildBrandSelectList(int? selectedId = null)
    {
        var brands = _brandRepository.GetAllAsync().GetAwaiter().GetResult();
        return brands
            .Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.BrandName,
                Selected = selectedId.HasValue && selectedId.Value == b.Id
            }).ToList();
    }

    private List<SelectListItem> BuildModelSelectList(int? selectedId = null)
    {
        var models = _modelRepository.GetAllAsync().GetAwaiter().GetResult();
        return models
            .Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.ModelName,
                Selected = selectedId.HasValue && selectedId.Value == m.Id
            }).ToList();
    }
}
