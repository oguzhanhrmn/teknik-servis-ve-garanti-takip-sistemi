namespace TSGTS.Core.Entities;

public class ServiceTicket
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int DeviceId { get; set; }
    public int OpenedByUserId { get; set; }
    public int StatusId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Description { get; set; }

    public Customer? Customer { get; set; }
    public Device? Device { get; set; }
    public User? OpenedByUser { get; set; }
    public TicketStatus? Status { get; set; }
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<ServicePartUsage> PartUsages { get; set; } = new List<ServicePartUsage>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();
}
