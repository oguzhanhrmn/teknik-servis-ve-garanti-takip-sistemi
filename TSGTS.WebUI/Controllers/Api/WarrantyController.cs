using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSGTS.DataAccess.Repositories;
using TSGTS.Core.Entities;

namespace TSGTS.WebUI.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class WarrantyController : ControllerBase
{
    private readonly IGenericRepository<Device> _deviceRepository;

    public WarrantyController(IGenericRepository<Device> deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    [HttpGet("check/{serialNumber}")]
    public async Task<IActionResult> Check(string serialNumber)
    {
        var device = (await _deviceRepository.FindAsync(d => d.SerialNumber == serialNumber)).FirstOrDefault();
        if (device is null)
            return NotFound(new { serialNumber, message = "Cihaz bulunamadı" });

        var isUnderWarranty = device.WarrantyEndDate.HasValue && device.WarrantyEndDate.Value >= DateTime.UtcNow.Date;
        return Ok(new
        {
            serialNumber,
            warrantyEndDate = device.WarrantyEndDate,
            isUnderWarranty
        });
    }
}
