using Microsoft.AspNetCore.Mvc.Rendering;

namespace TSGTS.WebUI.Models;

public class TicketStatusUpdateViewModel
{
    public int TicketId { get; set; }
    public int StatusId { get; set; }
    public string? ActionLog { get; set; }
    public List<SelectListItem> Statuses { get; set; } = new();
}
