using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSGTS.Business.Interfaces;
using TSGTS.WebUI.Models;
using TSGTS.DataAccess.Repositories;
using TSGTS.Core.Entities;
using TSGTS.Core.DTOs;

namespace TSGTS.WebUI.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin,Teknisyen")]
public class TicketsController : Controller
{
    private readonly ITicketService _ticketService;
    private readonly ITicketStatusService _ticketStatusService;
    private readonly ICustomerService _customerService;
    private readonly IDeviceService _deviceService;
    private readonly IGenericRepository<Customer> _customerRepo;
    private readonly IGenericRepository<Device> _deviceRepo;
    private readonly IGenericRepository<Brand> _brandRepo;
    private readonly IGenericRepository<Model> _modelRepo;

    public TicketsController(
        ITicketService ticketService,
        ITicketStatusService ticketStatusService,
        ICustomerService customerService,
        IDeviceService deviceService,
        IGenericRepository<Customer> customerRepo,
        IGenericRepository<Device> deviceRepo,
        IGenericRepository<Brand> brandRepo,
        IGenericRepository<Model> modelRepo)
    {
        _ticketService = ticketService;
        _ticketStatusService = ticketStatusService;
        _customerService = customerService;
        _deviceService = deviceService;
        _customerRepo = customerRepo;
        _deviceRepo = deviceRepo;
        _brandRepo = brandRepo;
        _modelRepo = modelRepo;
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

    [HttpGet]
    public async Task<IActionResult> QuickCreate()
    {
        var vm = new QuickTicketViewModel();
        await FillDropdowns();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuickCreate(QuickTicketViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await FillDropdowns();
            return View(vm);
        }

        var normPhone = NormalizeContact(vm.CustomerPhone);
        var normEmail = NormalizeContact(vm.CustomerEmail);

        var existingCustomer = (await _customerRepo.FindAsync(c =>
            (c.Phone != null && NormalizeContact(c.Phone) == normPhone) ||
            (c.Email != null && NormalizeContact(c.Email) == normEmail))).FirstOrDefault();

        Customer customer;
        if (existingCustomer is not null)
        {
            customer = existingCustomer;
        }
        else
        {
            customer = new Customer
            {
                FirstName = vm.CustomerFirstName,
                LastName = vm.CustomerLastName,
                Phone = vm.CustomerPhone,
                Email = vm.CustomerEmail,
                Address = vm.CustomerAddress,
                TaxNo = vm.CustomerTaxNo
            };
            await _customerRepo.AddAsync(customer);
            await _customerRepo.SaveChangesAsync();
        }

        var existingDevice = (await _deviceRepo.FindAsync(d => d.SerialNumber == vm.DeviceSerial)).FirstOrDefault();
        Device device;
        if (existingDevice is not null)
        {
            device = existingDevice;
        }
        else
        {
            device = new Device
            {
                SerialNumber = vm.DeviceSerial,
                BrandId = vm.BrandId,
                ModelId = vm.ModelId,
                PurchaseDate = vm.PurchaseDate,
                WarrantyEndDate = vm.WarrantyEndDate
            };
            await _deviceRepo.AddAsync(device);
            await _deviceRepo.SaveChangesAsync();
        }

        var statusDefault = (await _ticketStatusService.GetAllAsync()).FirstOrDefault();
        var created = await _ticketService.CreateAsync(new ServiceTicketCreateDto
        {
            CustomerId = customer.Id,
            DeviceId = device.Id,
            OpenedByUserId = 1, // basit atama; gerekirse User.Claims'ten alınabilir
            StatusId = vm.StatusId != 0 ? vm.StatusId : statusDefault?.Id ?? 1,
            Description = vm.Description
        });

        // Kayıt oluşturulduktan sonra listeye yönlendir
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

    private async Task FillDropdowns()
    {
        ViewBag.Brands = (await _brandRepo.GetAllAsync())
            .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.BrandName })
            .ToList();
        ViewBag.Models = (await _modelRepo.GetAllAsync())
            .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.ModelName })
            .ToList();
        ViewBag.Statuses = (await _ticketStatusService.GetAllAsync())
            .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.StatusName })
            .ToList();
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
