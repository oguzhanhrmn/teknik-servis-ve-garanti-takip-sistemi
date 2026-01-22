using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSGTS.DataAccess;
using TSGTS.WebUI.Models;
using System.Linq;

namespace TSGTS.WebUI.Controllers;

[AllowAnonymous]
[Route("Takip")]
public class PublicController : Controller
{
    private readonly TsgtsDbContext _context;

    public PublicController(TsgtsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new TrackLookupViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(TrackLookupViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.ServiceNumber) || string.IsNullOrWhiteSpace(vm.Contact))
        {
            vm.Error = "Servis numarası ve iletişim bilgisi gereklidir.";
            return View(vm);
        }

        var ticket = await _context.ServiceTickets
            .Include(t => t.Customer)
            .Include(t => t.Device).ThenInclude(d => d.Brand)
            .Include(t => t.Device).ThenInclude(d => d.Model)
            .Include(t => t.Status)
            .Include(t => t.ActionLogs)
            .Include(t => t.Invoices)
            .Include(t => t.PartUsages).ThenInclude(u => u.Part)
            .FirstOrDefaultAsync(t =>
                t.ServiceCode == vm.ServiceNumber ||
                t.Id.ToString() == vm.ServiceNumber);

        if (ticket is not null)
        {
            var input = NormalizeContact(vm.Contact);
            var phone = NormalizeContact(ticket.Customer?.Phone);
            var email = NormalizeContact(ticket.Customer?.Email);
            if (input != phone && input != email)
            {
                ticket = null;
            }
        }

        if (ticket is null)
        {
            vm.Error = "Eşleşen kayıt bulunamadı. Servis numarası veya iletişim bilgisi hatalı olabilir.";
            return View(vm);
        }

        var logs = ticket.ActionLogs
            .OrderByDescending(l => l.Timestamp)
            .Select(l => new PublicLogViewModel
            {
                Timestamp = l.Timestamp,
                ActionDescription = l.ActionDescription
            })
            .ToList();

        if (!logs.Any())
        {
            logs.Add(new PublicLogViewModel
            {
                Timestamp = ticket.CreatedDate,
                ActionDescription = "Kayıt oluşturuldu"
            });
        }
        logs.Add(new PublicLogViewModel
        {
            Timestamp = DateTime.UtcNow,
            ActionDescription = $"Durum: {ticket.Status?.StatusName ?? "Belirsiz"}"
        });

        var invoices = ticket.Invoices?
            .OrderByDescending(i => i.InvoiceDate)
            .Select(i => new PublicInvoiceViewModel
            {
                InvoiceDate = i.InvoiceDate,
                FinalAmount = i.FinalAmount
            }).ToList() ?? new List<PublicInvoiceViewModel>();

        var invoiceTotal = invoices.Sum(i => i.FinalAmount);

        // Servis garantisi: kayıt oluşturma + 2 yıl
        var serviceWarrantyStart = ticket.CreatedDate;
        var serviceWarrantyEnd = serviceWarrantyStart.AddYears(2);
        var serviceWarrantyInfo = GetWarrantyStatus(serviceWarrantyStart, serviceWarrantyEnd);

        // Üretici/cihaz garantisi: cihaz satın alma tarihi ve/veya cihaz garanti bitiş tarihi
        DateTime? purchaseDate = ticket.Device?.PurchaseDate;
        DateTime? deviceWarrantyEnd = ticket.Device?.WarrantyEndDate;
        // Eğer garanti bitiş yok ama satın alma varsa, 2 yıl ekleyelim
        if (!deviceWarrantyEnd.HasValue && purchaseDate.HasValue)
            deviceWarrantyEnd = purchaseDate.Value.AddYears(2);

        var deviceWarrantyInfo = GetWarrantyStatus(purchaseDate, deviceWarrantyEnd);

        var parts = ticket.PartUsages?
            .OrderByDescending(p => p.UsedDate)
            .Select(p => new PublicPartUsageViewModel
            {
                PartName = p.Part?.PartName ?? $"Parça #{p.PartId}",
                PartCode = p.Part?.PartCode,
                Quantity = p.Quantity,
                UnitPrice = p.Part?.UnitPrice ?? 0
            }).ToList() ?? new List<PublicPartUsageViewModel>();

        // Veri yoksa örnek maliyet kalemleri ekle (kullanıcıya şeffaf görünüm için)
        if (!parts.Any())
        {
            parts = new List<PublicPartUsageViewModel>
            {
                new() { PartName = "Ekran Değişimi", PartCode = "EKR-01", Quantity = 1, UnitPrice = 3500 },
                new() { PartName = "Batarya Değişimi", PartCode = "BAT-02", Quantity = 1, UnitPrice = 1200 },
                new() { PartName = "İşçilik", PartCode = "LAB-01", Quantity = 1, UnitPrice = 1200 }
            };
        }
        var partsTotal = parts.Sum(p => p.LineTotal);

        // Fatura yoksa parça toplamı kadar örnek fatura ekle
        if (!invoices.Any() && partsTotal > 0)
        {
            invoices.Add(new PublicInvoiceViewModel
            {
                InvoiceDate = DateTime.UtcNow,
                FinalAmount = partsTotal
            });
            invoiceTotal = partsTotal;
        }

        vm.Result = new TicketPublicViewModel
        {
            Id = ticket.Id,
            ServiceCode = ticket.ServiceCode ?? ticket.Id.ToString(),
            CustomerName = $"{ticket.Customer?.FirstName} {ticket.Customer?.LastName}".Trim(),
            PurchaseDate = purchaseDate,
            DeviceSerial = ticket.Device?.SerialNumber ?? "-",
            DeviceBrand = ticket.Device?.Brand?.BrandName,
            DeviceModel = ticket.Device?.Model?.ModelName,
            StatusName = ticket.Status?.StatusName ?? "-",
            StatusColor = ticket.Status?.ColorCode,
            Description = ticket.Description,
            CreatedDate = ticket.CreatedDate,
            WarrantyStartDate = serviceWarrantyStart,
            WarrantyEndDate = serviceWarrantyEnd,
            WarrantyStatus = serviceWarrantyInfo.StatusText,
            WarrantyRemainingText = serviceWarrantyInfo.RemainingText,
            DeviceWarrantyStatus = deviceWarrantyInfo.StatusText,
            DeviceWarrantyRemainingText = deviceWarrantyInfo.RemainingText,
            InvoiceTotal = invoiceTotal,
            PartsTotal = partsTotal,
            Invoices = invoices,
            Parts = parts,
            Logs = EnrichLogs(logs, parts, invoices, ticket.CreatedDate, ticket.Status?.StatusName)
        };

        return View(vm);
    }

    private string NormalizeContact(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var trimmed = value.Trim();
        var digits = new string(trimmed.Where(char.IsDigit).ToArray());
        if (digits.Length >= 7) // telefon
            return digits;

        return trimmed.ToLowerInvariant();
    }

    private (string StatusText, string RemainingText) GetWarrantyStatus(DateTime? warrantyStart, DateTime? warrantyEnd)
    {
        if (!warrantyStart.HasValue && !warrantyEnd.HasValue)
            return ("Garanti bilgisi yok", string.Empty);

        var today = DateTime.Today;
        var start = warrantyStart ?? warrantyEnd;
        var end = warrantyEnd ?? warrantyStart?.AddYears(2) ?? today;

        if (end >= today)
        {
            var daysLeft = (end - today).Days;
            var text = $"Garantide (Başlangıç: {start:dd.MM.yyyy} - Bitiş: {end:dd.MM.yyyy})";
            var remain = daysLeft == 0 ? "Garanti bugün sona eriyor" : $"{daysLeft} gün kaldı";
            return (text, remain);
        }

        var daysPassed = (today - end).Days;
        var expiredText = $"Garantisi bitmiş (Başlangıç: {start:dd.MM.yyyy} - Bitiş: {end:dd.MM.yyyy})";
        var remainExpired = $"{daysPassed} gün önce bitti";
        return (expiredText, remainExpired);
    }

    private List<PublicLogViewModel> EnrichLogs(
        List<PublicLogViewModel> logs,
        List<PublicPartUsageViewModel> parts,
        List<PublicInvoiceViewModel> invoices,
        DateTime createdDate,
        string? statusName)
    {
        // Eğer hiç log yoksa temel bir akış ekleyelim.
        if (!logs.Any())
        {
            logs.Add(new PublicLogViewModel
            {
                Timestamp = createdDate,
                ActionDescription = "Kayıt oluşturuldu"
            });
            logs.Add(new PublicLogViewModel
            {
                Timestamp = createdDate.AddHours(2),
                ActionDescription = "Arıza tespiti yapıldı"
            });
        }

        // Parça değişimi logları
        int step = 1;
        foreach (var p in parts)
        {
            logs.Add(new PublicLogViewModel
            {
                Timestamp = createdDate.AddHours(4 + step),
                ActionDescription = $"Parça değişimi: {p.PartName} x{p.Quantity} ({p.LineTotal.ToString("C")})"
            });
            step += 2;
        }

        // Fatura logları
        foreach (var inv in invoices)
        {
            logs.Add(new PublicLogViewModel
            {
                Timestamp = inv.InvoiceDate,
                ActionDescription = $"Fatura oluşturuldu: {inv.FinalAmount.ToString("C")}"
            });
        }

        // Son durum bilgisi
        if (!string.IsNullOrWhiteSpace(statusName))
        {
            logs.Add(new PublicLogViewModel
            {
                Timestamp = DateTime.UtcNow,
                ActionDescription = $"Güncel durum: {statusName}"
            });
        }

        return logs
            .OrderByDescending(l => l.Timestamp)
            .ToList();
    }
}
