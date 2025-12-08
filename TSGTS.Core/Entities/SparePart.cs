namespace TSGTS.Core.Entities;

public class SparePart
{
    public int Id { get; set; }
    public string PartName { get; set; } = string.Empty;
    public string PartCode { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int CriticalLevel { get; set; }

    public ICollection<ServicePartUsage> PartUsages { get; set; } = new List<ServicePartUsage>();
}
