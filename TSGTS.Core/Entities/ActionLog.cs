namespace TSGTS.Core.Entities;

public class ActionLog
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string ActionDescription { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    public ServiceTicket? Ticket { get; set; }
    public User? User { get; set; }
}
