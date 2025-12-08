namespace TSGTS.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;

    public Role? Role { get; set; }
    public ICollection<ServiceTicket> OpenedTickets { get; set; } = new List<ServiceTicket>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();
}
