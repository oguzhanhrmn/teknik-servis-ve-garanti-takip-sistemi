using TSGTS.Core.DTOs;

namespace TSGTS.Business.Interfaces;

public interface ITicketStatusService
{
    Task<IEnumerable<TicketStatusDto>> GetAllAsync();
}
