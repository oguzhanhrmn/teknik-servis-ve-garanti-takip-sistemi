namespace TSGTS.Core.Entities;

public class Payment
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int PaymentTypeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public Invoice? Invoice { get; set; }
    public PaymentType? PaymentType { get; set; }
}
