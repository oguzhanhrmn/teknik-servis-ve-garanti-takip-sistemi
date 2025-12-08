namespace TSGTS.Core.Entities;

public class PaymentType
{
    public int Id { get; set; }
    public string TypeName { get; set; } = string.Empty;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
