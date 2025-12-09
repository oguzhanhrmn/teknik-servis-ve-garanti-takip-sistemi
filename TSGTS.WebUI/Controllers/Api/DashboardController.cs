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

    public DashboardController(
        IGenericRepository<ServiceTicket> ticketRepo,
        IGenericRepository<Customer> customerRepo,
        IGenericRepository<Device> deviceRepo)
    {
        _ticketRepo = ticketRepo;
        _customerRepo = customerRepo;
        _deviceRepo = deviceRepo;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var tickets = await _ticketRepo.GetAllAsync();
        var customers = await _customerRepo.GetAllAsync();
        var devices = await _deviceRepo.GetAllAsync();

        var statusCounts = tickets
            .GroupBy(t => t.StatusId)
            .Select(g => new { statusId = g.Key, count = g.Count() })
            .ToList();

        return Ok(new
        {
            totalTickets = tickets.Count(),
            totalCustomers = customers.Count(),
            totalDevices = devices.Count(),
            statusCounts
        });
    }
}
