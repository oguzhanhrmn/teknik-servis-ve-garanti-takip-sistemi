using Microsoft.AspNetCore.Mvc;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;

namespace TSGTS.WebUI.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _deviceService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var device = await _deviceService.GetByIdAsync(id);
        if (device is null)
            return NotFound();
        return Ok(device);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DeviceCreateDto dto)
    {
        var created = await _deviceService.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DeviceCreateDto dto)
    {
        var updated = await _deviceService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _deviceService.DeleteAsync(id);
        if (!ok)
            return NotFound();
        return NoContent();
    }
}
