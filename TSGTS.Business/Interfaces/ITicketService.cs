using TSGTS.Core.DTOs;

namespace TSGTS.Business.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<ServiceTicketDto>> GetAllAsync();
    Task<ServiceTicketDto?> GetByIdAsync(int id);
    Task<ServiceTicketDto> CreateAsync(ServiceTicketCreateDto dto);
    Task<ServiceTicketDto?> UpdateStatusAsync(int id, int statusId, string? actionLog = null, int? userId = null);
    Task<bool> DeleteAsync(int id);
}
