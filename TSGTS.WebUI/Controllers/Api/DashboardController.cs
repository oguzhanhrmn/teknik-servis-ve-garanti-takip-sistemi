using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSGTS.DataAccess.Repositories;
using TSGTS.Core.Entities;

namespace TSGTS.WebUI.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DashboardController : ControllerBase
{
    private readonly IGenericRepository<ServiceTicket> _ticketRepo;
    private readonly IGenericRepository<Customer> _customerRepo;
    private readonly IGenericRepository<Device> _deviceRepo;
    private readonly IGenericRepository<TicketStatus> _statusRepo;
    private readonly IGenericRepository<SparePart> _sparePartRepo;
    private readonly IGenericRepository<Invoice> _invoiceRepo;
    private readonly IGenericRepository<Assignment> _assignmentRepo;
    private readonly IGenericRepository<User> _userRepo;

    public DashboardController(
        IGenericRepository<ServiceTicket> ticketRepo,
        IGenericRepository<Customer> customerRepo,
        IGenericRepository<Device> deviceRepo,
        IGenericRepository<TicketStatus> statusRepo,
        IGenericRepository<SparePart> sparePartRepo,
        IGenericRepository<Invoice> invoiceRepo,
        IGenericRepository<Assignment> assignmentRepo,
        IGenericRepository<User> userRepo)
    {
        _ticketRepo = ticketRepo;
        _customerRepo = customerRepo;
        _deviceRepo = deviceRepo;
        _statusRepo = statusRepo;
        _sparePartRepo = sparePartRepo;
        _invoiceRepo = invoiceRepo;
        _assignmentRepo = assignmentRepo;
        _userRepo = userRepo;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var tickets = await _ticketRepo.GetAllAsync();
        var customers = await _customerRepo.GetAllAsync();
        var devices = await _deviceRepo.GetAllAsync();
        var statuses = (await _statusRepo.GetAllAsync())
            .ToDictionary(s => s.Id, s => new { s.StatusName, s.ColorCode });
        var customerMap = customers.ToDictionary(c => c.Id, c => $"{c.FirstName} {c.LastName}");
        var deviceMap = devices.ToDictionary(d => d.Id, d => d.SerialNumber);
        var spareParts = await _sparePartRepo.GetAllAsync();
        var invoices = await _invoiceRepo.GetAllAsync();
        var assignments = await _assignmentRepo.GetAllAsync();
        var users = await _userRepo.GetAllAsync();

        var statusCounts = tickets
            .GroupBy(t => t.StatusId)
            .Select(g => new
            {
                statusId = g.Key,
                statusName = statuses.TryGetValue(g.Key, out var s)
                    ? (s?.StatusName ?? $"Durum {g.Key}")
                    : $"Durum {g.Key}",
                colorCode = statuses.TryGetValue(g.Key, out var sc)
                    ? (string.IsNullOrWhiteSpace(sc?.ColorCode) ? null : sc!.ColorCode)
                    : null,
                count = g.Count()
            })
            .ToList();

        var last7 = Enumerable.Range(0, 7)
            .Select(i => DateTime.UtcNow.Date.AddDays(-i))
            .OrderBy(d => d)
            .ToList();

        var ticketTrend = last7.Select(d => new
        {
            date = d.ToString("yyyy-MM-dd"),
            count = tickets.Count(t => t.CreatedDate.Date == d)
        }).ToList();

        var revenueTrend = invoices
            .GroupBy(i => new { i.InvoiceDate.Year, i.InvoiceDate.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .Select(g => new
            {
                label = $"{g.Key.Year}-{g.Key.Month:D2}",
                total = g.Sum(x => x.FinalAmount)
            })
            .TakeLast(6)
            .ToList();

        var workload = assignments
            .GroupBy(a => a.TechnicianId)
            .Select(g => new
            {
                technicianId = g.Key,
                technician = users.FirstOrDefault(u => u.Id == g.Key)?.Username ?? $"Teknisyen #{g.Key}",
                count = g.Count()
            })
            .OrderByDescending(x => x.count)
            .ToList();

        var recentTickets = tickets
            .OrderByDescending(t => t.CreatedDate)
            .Take(5)
            .Select(t => new
            {
                t.Id,
                customer = customerMap.TryGetValue(t.CustomerId, out var c) ? c : $"Müşteri #{t.CustomerId}",
                device = deviceMap.TryGetValue(t.DeviceId, out var d) ? d : $"Cihaz #{t.DeviceId}",
                statusName = statuses.TryGetValue(t.StatusId, out var s) ? s?.StatusName ?? $"Durum {t.StatusId}" : $"Durum {t.StatusId}",
                statusColor = statuses.TryGetValue(t.StatusId, out var sc) && !string.IsNullOrWhiteSpace(sc?.ColorCode) ? sc!.ColorCode : null,
                t.CreatedDate,
                t.Description
            })
            .ToList();

        var lowStockParts = spareParts
            .Where(p => p.StockQuantity <= p.CriticalLevel)
            .Select(p => new
            {
                p.PartName,
                p.PartCode,
                p.StockQuantity,
                p.CriticalLevel
            }).ToList();

        return Ok(new
        {
            totalTickets = tickets.Count(),
            totalCustomers = customers.Count(),
            totalDevices = devices.Count(),
            statusCounts,
            recentTickets,
            ticketTrend,
            lowStockParts,
            revenueTrend,
            workload
        });
    }
}
