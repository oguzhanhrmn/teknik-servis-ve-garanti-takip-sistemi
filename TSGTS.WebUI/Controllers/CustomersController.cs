using Microsoft.AspNetCore.Mvc;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;

namespace TSGTS.WebUI.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IActionResult> Index()
    {
        var customers = await _customerService.GetAllAsync();
        return View(customers);
    }

    public IActionResult Create()
    {
        return View(new CustomerCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _customerService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer is null)
            return NotFound();

        var editDto = new CustomerCreateDto
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address,
            TaxNo = customer.TaxNo
        };

        ViewBag.CustomerId = id;
        return View(editDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CustomerCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CustomerId = id;
            return View(dto);
        }

        var updated = await _customerService.UpdateAsync(id, dto);
        if (updated is null)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _customerService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
