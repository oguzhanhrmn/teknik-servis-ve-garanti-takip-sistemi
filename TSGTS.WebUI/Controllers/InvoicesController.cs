using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;

namespace TSGTS.WebUI.Controllers;

[Authorize(Roles = "Admin")]
public class InvoicesController : Controller
{
    private readonly IInvoiceService _invoiceService;
    private readonly ITicketService _ticketService;

    public InvoicesController(IInvoiceService invoiceService, ITicketService ticketService)
    {
        _invoiceService = invoiceService;
        _ticketService = ticketService;
    }

    public async Task<IActionResult> Index()
    {
        var invoices = await _invoiceService.GetAllAsync();
        return View(invoices);
    }

    public async Task<IActionResult> Create()
    {
        var tickets = await _ticketService.GetAllAsync();
        ViewBag.Tickets = tickets.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = $"#{t.Id} - {t.Description}" }).ToList();
        return View(new InvoiceCreateDto { InvoiceDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InvoiceCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            var tickets = await _ticketService.GetAllAsync();
            ViewBag.Tickets = tickets.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = $"#{t.Id} - {t.Description}" }).ToList();
            return View(dto);
        }

        await _invoiceService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _invoiceService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
