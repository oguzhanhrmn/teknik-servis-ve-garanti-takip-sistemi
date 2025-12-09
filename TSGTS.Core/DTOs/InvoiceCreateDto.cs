namespace TSGTS.Core.DTOs;

public class InvoiceCreateDto
{
    public int TicketId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public DateTime InvoiceDate { get; set; }
}
