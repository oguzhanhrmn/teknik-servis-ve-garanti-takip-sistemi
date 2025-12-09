namespace TSGTS.WebUI.Models;

public class TicketListItemViewModel
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public string? DeviceSerial { get; set; }
    public string? StatusName { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
}
