using Microsoft.AspNetCore.Mvc.Rendering;
using TSGTS.Core.DTOs;

namespace TSGTS.WebUI.Models;

public class TicketCreateViewModel
{
    public ServiceTicketCreateDto Ticket { get; set; } = new();
    public List<SelectListItem> Customers { get; set; } = new();
    public List<SelectListItem> Devices { get; set; } = new();
    public List<SelectListItem> Statuses { get; set; } = new();
}
