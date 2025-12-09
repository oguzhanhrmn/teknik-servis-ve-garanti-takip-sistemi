using Microsoft.AspNetCore.Mvc;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;

namespace TSGTS.WebUI.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _ticketService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket is null)
            return NotFound();
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceTicketCreateDto dto)
    {
        var created = await _ticketService.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    public class TicketStatusUpdateRequest
    {
        public int StatusId { get; set; }
        public string? ActionLog { get; set; }
        public int? UserId { get; set; }
    }

    [HttpPost("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TicketStatusUpdateRequest request)
    {
        var updated = await _ticketService.UpdateStatusAsync(id, request.StatusId, request.ActionLog, request.UserId);
        if (updated is null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _ticketService.DeleteAsync(id);
        if (!ok)
            return NotFound();
        return NoContent();
    }
}
