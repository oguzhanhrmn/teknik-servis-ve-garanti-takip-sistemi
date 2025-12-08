namespace TSGTS.Core.Entities;

public class ServicePartUsage
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public DateTime UsedDate { get; set; }

    public ServiceTicket? Ticket { get; set; }
    public SparePart? Part { get; set; }
}
