namespace TSGTS.Core.Entities;

public class Assignment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int TechnicianId { get; set; }
    public DateTime AssignedDate { get; set; }

    public ServiceTicket? Ticket { get; set; }
    public User? Technician { get; set; }
}
