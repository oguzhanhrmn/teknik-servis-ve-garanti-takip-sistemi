using System.ComponentModel.DataAnnotations;

namespace TSGTS.WebUI.Models;

public class TrackLookupViewModel
{
    [Display(Name = "Servis No")]
    public string? ServiceNumber { get; set; }

    [Display(Name = "Telefon veya E-posta")]
    public string? Contact { get; set; }

    public TicketPublicViewModel? Result { get; set; }
    public string? Error { get; set; }
}

public class TicketPublicViewModel
{
    public int Id { get; set; }
    public string ServiceCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime? PurchaseDate { get; set; }
    public string DeviceSerial { get; set; } = string.Empty;
    public string? DeviceBrand { get; set; }
    public string? DeviceModel { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? StatusColor { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? WarrantyStartDate { get; set; } // Servis garantisi başlangıcı
    public DateTime? WarrantyEndDate { get; set; }   // Servis garantisi bitişi (+2 yıl)
    public string WarrantyStatus { get; set; } = string.Empty; // Servis garantisi metni
    public string WarrantyRemainingText { get; set; } = string.Empty; // Servis garantisi kalan gün

    public string DeviceWarrantyStatus { get; set; } = string.Empty; // Üretici/ürün garantisi metni
    public string DeviceWarrantyRemainingText { get; set; } = string.Empty; // Üretici garantisi kalan gün
    public decimal InvoiceTotal { get; set; }
    public decimal PartsTotal { get; set; }
    public List<PublicInvoiceViewModel> Invoices { get; set; } = new();
    public List<PublicPartUsageViewModel> Parts { get; set; } = new();
    public List<PublicLogViewModel> Logs { get; set; } = new();
}

public class PublicLogViewModel
{
    public DateTime Timestamp { get; set; }
    public string ActionDescription { get; set; } = string.Empty;
}

public class PublicInvoiceViewModel
{
    public DateTime InvoiceDate { get; set; }
    public decimal FinalAmount { get; set; }
}

public class PublicPartUsageViewModel
{
    public string PartName { get; set; } = string.Empty;
    public string? PartCode { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}
