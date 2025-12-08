namespace TSGTS.Core.Entities;

public class Invoice
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public DateTime InvoiceDate { get; set; }

    public ServiceTicket? Ticket { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
