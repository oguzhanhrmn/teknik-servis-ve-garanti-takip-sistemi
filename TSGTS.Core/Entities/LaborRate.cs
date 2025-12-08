namespace TSGTS.Core.Entities;

public class LaborRate
{
    public int Id { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
}
