namespace TSGTS.Core.DTOs;

public class ServiceTicketDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int DeviceId { get; set; }
    public int OpenedByUserId { get; set; }
    public int StatusId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? ServiceCode { get; set; }
    public string? Description { get; set; }
}
