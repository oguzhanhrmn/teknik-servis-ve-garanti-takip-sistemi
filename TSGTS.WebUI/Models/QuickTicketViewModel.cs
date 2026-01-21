using System.ComponentModel.DataAnnotations;

namespace TSGTS.WebUI.Models;

public class QuickTicketViewModel
{
    // Müşteri
    [Required, Display(Name = "Ad"), MaxLength(75)]
    public string CustomerFirstName { get; set; } = string.Empty;

    [Required, Display(Name = "Soyad"), MaxLength(75)]
    public string CustomerLastName { get; set; } = string.Empty;

    [Required, Display(Name = "Telefon"), MaxLength(20)]
    public string CustomerPhone { get; set; } = string.Empty;

    [Display(Name = "E-posta"), MaxLength(100)]
    public string? CustomerEmail { get; set; }

    [Display(Name = "Adres"), MaxLength(200)]
    public string? CustomerAddress { get; set; }

    [Display(Name = "Vergi No"), MaxLength(20)]
    public string? CustomerTaxNo { get; set; }

    // Cihaz
    [Required, Display(Name = "Seri No"), MaxLength(100)]
    public string DeviceSerial { get; set; } = string.Empty;

    [Required, Display(Name = "Marka")]
    public int BrandId { get; set; }

    [Required, Display(Name = "Model")]
    public int ModelId { get; set; }

    [Display(Name = "Satın Alma Tarihi")]
    public DateTime? PurchaseDate { get; set; }

    [Display(Name = "Garanti Bitiş")]
    public DateTime? WarrantyEndDate { get; set; }

    // Servis Kaydı
    [Required, Display(Name = "Durum")]
    public int StatusId { get; set; }

    [Display(Name = "Açıklama"), MaxLength(1000)]
    public string? Description { get; set; }

    public string? CreatedServiceCode { get; set; }
}
