using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;

namespace TSGTS.WebUI.Controllers;

[Authorize(Roles = "Admin")]
public class PaymentsController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly IInvoiceService _invoiceService;
    private readonly IPaymentTypeService _paymentTypeService;

    public PaymentsController(IPaymentService paymentService, IInvoiceService invoiceService, IPaymentTypeService paymentTypeService)
    {
        _paymentService = paymentService;
        _invoiceService = invoiceService;
        _paymentTypeService = paymentTypeService;
    }

    public async Task<IActionResult> Index()
    {
        var payments = await _paymentService.GetAllAsync();
        return View(payments);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateLookups();
        return View(new PaymentCreateDto { Date = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await PopulateLookups();
            return View(dto);
        }

        await _paymentService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _paymentService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateLookups()
    {
        var invoices = await _invoiceService.GetAllAsync();
        var paymentTypes = await _paymentTypeService.GetAllAsync();

        ViewBag.Invoices = invoices.Select(i => new SelectListItem { Value = i.Id.ToString(), Text = $"Fatura #{i.Id} - {i.FinalAmount:C}" }).ToList();
        ViewBag.PaymentTypes = paymentTypes.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.TypeName }).ToList();
    }
}
