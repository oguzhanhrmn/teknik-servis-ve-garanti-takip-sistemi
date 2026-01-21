using AutoMapper;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;
using TSGTS.DataAccess.Repositories;

namespace TSGTS.Business.Services;

public class TicketManager : ITicketService
{
    private readonly IGenericRepository<ServiceTicket> _repository;
    private readonly IGenericRepository<ActionLog> _actionLogRepository;
    private readonly IMapper _mapper;

    public TicketManager(
        IGenericRepository<ServiceTicket> repository,
        IGenericRepository<ActionLog> actionLogRepository,
        IMapper mapper)
    {
        _repository = repository;
        _actionLogRepository = actionLogRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceTicketDto>> GetAllAsync()
    {
        var tickets = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ServiceTicketDto>>(tickets);
    }

    public async Task<ServiceTicketDto?> GetByIdAsync(int id)
    {
        var ticket = await _repository.GetByIdAsync(id);
        return ticket is null ? null : _mapper.Map<ServiceTicketDto>(ticket);
    }

    public async Task<ServiceTicketDto> CreateAsync(ServiceTicketCreateDto dto)
    {
        var entity = _mapper.Map<ServiceTicket>(dto);
        entity.CreatedDate = DateTime.UtcNow;
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        if (string.IsNullOrWhiteSpace(entity.ServiceCode))
        {
            entity.ServiceCode = $"SRV-{DateTime.UtcNow:yyyy}-{entity.Id:D5}";
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }
        return _mapper.Map<ServiceTicketDto>(entity);
    }

    public async Task<ServiceTicketDto?> UpdateStatusAsync(int id, int statusId, string? actionLog = null, int? userId = null)
    {
        var ticket = await _repository.GetByIdAsync(id);
        if (ticket is null)
            return null;

        ticket.StatusId = statusId;
        _repository.Update(ticket);

        if (!string.IsNullOrWhiteSpace(actionLog) && userId.HasValue)
        {
            var log = new ActionLog
            {
                TicketId = ticket.Id,
                UserId = userId.Value,
                ActionDescription = actionLog,
                Timestamp = DateTime.UtcNow
            };
            await _actionLogRepository.AddAsync(log);
        }

        await _repository.SaveChangesAsync();
        return _mapper.Map<ServiceTicketDto>(ticket);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return false;

        _repository.Delete(existing);
        await _repository.SaveChangesAsync();
        return true;
    }
}
