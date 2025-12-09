namespace TSGTS.Core.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int PaymentTypeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
