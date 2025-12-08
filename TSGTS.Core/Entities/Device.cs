namespace TSGTS.Core.Entities;

public class Device
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public int BrandId { get; set; }
    public int ModelId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? WarrantyEndDate { get; set; }

    public Brand? Brand { get; set; }
    public Model? Model { get; set; }
    public ICollection<ServiceTicket> ServiceTickets { get; set; } = new List<ServiceTicket>();
}
