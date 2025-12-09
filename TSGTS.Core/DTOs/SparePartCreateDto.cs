namespace TSGTS.Core.DTOs;

public class SparePartCreateDto
{
    public string PartName { get; set; } = string.Empty;
    public string PartCode { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int CriticalLevel { get; set; }
}
