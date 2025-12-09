namespace TSGTS.Core.DTOs;

public class TicketStatusDto
{
    public int Id { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? ColorCode { get; set; }
}
