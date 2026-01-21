using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSGTS.DataAccess;

namespace TSGTS.WebUI.Controllers.Api;

[ApiController]
[Route("api/public/tickets")]
[AllowAnonymous]
public class PublicTicketsController : ControllerBase
{
    private readonly TsgtsDbContext _context;
    public PublicTicketsController(TsgtsDbContext context)
    {
        _context = context;
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup([FromQuery] string code, [FromQuery] string contact)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(contact))
            return BadRequest("code ve contact gereklidir.");

        var ticket = await _context.ServiceTickets
            .Include(t => t.Customer)
            .Include(t => t.Device).ThenInclude(d => d.Brand)
            .Include(t => t.Device).ThenInclude(d => d.Model)
            .Include(t => t.Status)
            .Include(t => t.ActionLogs)
            .FirstOrDefaultAsync(t =>
                t.ServiceCode == code || t.Id.ToString() == code);

        if (ticket is null)
            return NotFound();

        var input = NormalizeContact(contact);
        var phone = NormalizeContact(ticket.Customer?.Phone);
        var email = NormalizeContact(ticket.Customer?.Email);
        if (input != phone && input != email)
            return Unauthorized();

        var logs = ticket.ActionLogs
            .OrderByDescending(l => l.Timestamp)
            .Select(l => new
            {
                l.Timestamp,
                l.ActionDescription
            })
            .ToList();

        if (!logs.Any())
        {
            logs.Add(new { Timestamp = ticket.CreatedDate, ActionDescription = "Kayıt oluşturuldu" });
        }

        var currentStatus = ticket.Status?.StatusName ?? "Belirsiz";
        logs.Insert(0, new { Timestamp = DateTime.UtcNow, ActionDescription = $"Durum: {currentStatus}" });

        return Ok(new
        {
            ticket.Id,
            ServiceCode = ticket.ServiceCode ?? ticket.Id.ToString(),
            Customer = $"{ticket.Customer?.FirstName} {ticket.Customer?.LastName}".Trim(),
            DeviceSerial = ticket.Device?.SerialNumber,
            DeviceBrand = ticket.Device?.Brand?.BrandName,
            DeviceModel = ticket.Device?.Model?.ModelName,
            Status = ticket.Status?.StatusName,
            StatusColor = ticket.Status?.ColorCode,
            Description = ticket.Description,
            CreatedDate = ticket.CreatedDate,
            WarrantyEndDate = ticket.Device?.WarrantyEndDate,
            Logs = logs
        });
    }

    [HttpGet("{id:int}/timeline")]
    public async Task<IActionResult> Timeline(int id, [FromQuery] string contact)
    {
        if (string.IsNullOrWhiteSpace(contact))
            return BadRequest("contact gereklidir.");

        var ticket = await _context.ServiceTickets
            .Include(t => t.Customer)
            .Include(t => t.Status)
            .Include(t => t.ActionLogs)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket is null)
            return NotFound();

        var input = NormalizeContact(contact);
        var phone = NormalizeContact(ticket.Customer?.Phone);
        var email = NormalizeContact(ticket.Customer?.Email);
        if (input != phone && input != email)
            return Unauthorized();

        var logs = ticket.ActionLogs
            .OrderByDescending(l => l.Timestamp)
            .Select(l => new { l.Timestamp, l.ActionDescription })
            .ToList();

        if (!logs.Any())
            logs.Add(new { Timestamp = ticket.CreatedDate, ActionDescription = "Kayıt oluşturuldu" });

        var currentStatus = ticket.Status?.StatusName ?? "Belirsiz";
        logs.Insert(0, new { Timestamp = DateTime.UtcNow, ActionDescription = $"Durum: {currentStatus}" });

        return Ok(logs);
    }

    private string NormalizeContact(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var trimmed = value.Trim();
        var digits = new string(trimmed.Where(char.IsDigit).ToArray());
        if (digits.Length >= 7)
            return digits;
        return trimmed.ToLowerInvariant();
    }
}
