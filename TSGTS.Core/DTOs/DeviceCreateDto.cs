namespace TSGTS.Core.DTOs;

public class DeviceCreateDto
{
    public string SerialNumber { get; set; } = string.Empty;
    public int BrandId { get; set; }
    public int ModelId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? WarrantyEndDate { get; set; }
}
