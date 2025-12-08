namespace TSGTS.Core.Entities;

public class TicketStatus
{
    public int Id { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? ColorCode { get; set; }

    public ICollection<ServiceTicket> ServiceTickets { get; set; } = new List<ServiceTicket>();
}
