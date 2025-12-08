namespace TSGTS.Core.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? TaxNo { get; set; }

    public ICollection<ServiceTicket> ServiceTickets { get; set; } = new List<ServiceTicket>();
}
