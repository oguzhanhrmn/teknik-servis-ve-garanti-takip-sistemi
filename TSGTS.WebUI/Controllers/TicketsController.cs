using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSGTS.Business.Interfaces;
using TSGTS.WebUI.Models;

namespace TSGTS.WebUI.Controllers;

public class TicketsController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly ITicketStatusService _ticketStatusService;
    private readonly ICustomerService _customerService;
    private readonly IDeviceService _deviceService;

    public TicketsController(
        ITicketService ticketService,
        ITicketStatusService ticketStatusService,
        ICustomerService customerService,
        IDeviceService deviceService)
    {
        _ticketService = ticketService;
        _ticketStatusService = ticketStatusService;
        _customerService = customerService;
        _deviceService = deviceService;
    }

    public async Task<IActionResult> Index()
    {
        var tickets = await _ticketService.GetAllAsync();
        var customers = (await _customerService.GetAllAsync()).ToDictionary(c => c.Id);
        var devices = (await _deviceService.GetAllAsync()).ToDictionary(d => d.Id);
        var statuses = (await _ticketStatusService.GetAllAsync()).ToDictionary(s => s.Id);

        var vm = tickets.Select(t => new TicketListItemViewModel
        {
            Id = t.Id,
            CustomerName = customers.TryGetValue(t.CustomerId, out var c) ? $"{c.FirstName} {c.LastName}" : $"#{t.CustomerId}",
            DeviceSerial = devices.TryGetValue(t.DeviceId, out var d) ? d.SerialNumber : $"#{t.DeviceId}",
            StatusName = statuses.TryGetValue(t.StatusId, out var s) ? s.StatusName : $"#{t.StatusId}",
            Description = t.Description,
            CreatedDate = t.CreatedDate
        }).ToList();

        return View(vm);
    }

    public async Task<IActionResult> Create()
    {
        var vm = await BuildCreateViewModel();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TicketCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm = await BuildCreateViewModel(vm);
            return View(vm);
        }

        await _ticketService.CreateAsync(vm.Ticket);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Status(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket is null)
            return NotFound();

        var statuses = await _ticketStatusService.GetAllAsync();
        var vm = new TicketStatusUpdateViewModel
        {
            TicketId = ticket.Id,
            StatusId = ticket.StatusId,
            Statuses = statuses
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StatusName, Selected = s.Id == ticket.StatusId })
                .ToList()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Status(TicketStatusUpdateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var statuses = await _ticketStatusService.GetAllAsync();
            vm.Statuses = statuses
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StatusName, Selected = s.Id == vm.StatusId })
                .ToList();
            return View(vm);
        }

        var updated = await _ticketService.UpdateStatusAsync(vm.TicketId, vm.StatusId, vm.ActionLog, null);
        if (updated is null)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _ticketService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task<TicketCreateViewModel> BuildCreateViewModel(TicketCreateViewModel? vm = null)
    {
        vm ??= new TicketCreateViewModel();

        var customers = await _customerService.GetAllAsync();
        var devices = await _deviceService.GetAllAsync();
        var statuses = await _ticketStatusService.GetAllAsync();

        vm.Customers = customers
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = $"{c.FirstName} {c.LastName}" })
            .ToList();

        vm.Devices = devices
            .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.SerialNumber })
            .ToList();

        vm.Statuses = statuses
            .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StatusName })
            .ToList();

        return vm;
    }
}
