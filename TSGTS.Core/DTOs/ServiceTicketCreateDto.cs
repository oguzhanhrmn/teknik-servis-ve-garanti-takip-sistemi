namespace TSGTS.Core.DTOs;

public class ServiceTicketCreateDto
{
    public int CustomerId { get; set; }
    public int DeviceId { get; set; }
    public int OpenedByUserId { get; set; }
    public int StatusId { get; set; }
    public string? Description { get; set; }
}
