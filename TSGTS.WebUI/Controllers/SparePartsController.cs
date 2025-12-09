using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;

namespace TSGTS.WebUI.Controllers;

[Authorize(Roles = "Admin")]
public class SparePartsController : Controller
{
    private readonly ISparePartService _sparePartService;

    public SparePartsController(ISparePartService sparePartService)
    {
        _sparePartService = sparePartService;
    }

    public async Task<IActionResult> Index()
    {
        var parts = await _sparePartService.GetAllAsync();
        return View(parts);
    }

    public IActionResult Create()
    {
        return View(new SparePartCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SparePartCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _sparePartService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var part = await _sparePartService.GetByIdAsync(id);
        if (part is null) return NotFound();

        var dto = new SparePartCreateDto
        {
            PartName = part.PartName,
            PartCode = part.PartCode,
            StockQuantity = part.StockQuantity,
            UnitPrice = part.UnitPrice,
            CriticalLevel = part.CriticalLevel
        };
        ViewBag.PartId = id;
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SparePartCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.PartId = id;
            return View(dto);
        }

        var updated = await _sparePartService.UpdateAsync(id, dto);
        if (updated is null) return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _sparePartService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
