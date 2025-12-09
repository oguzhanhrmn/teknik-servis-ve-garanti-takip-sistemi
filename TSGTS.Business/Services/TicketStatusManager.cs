using AutoMapper;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;
using TSGTS.DataAccess.Repositories;

namespace TSGTS.Business.Services;

public class TicketStatusManager : ITicketStatusService
{
    private readonly IGenericRepository<TicketStatus> _repository;
    private readonly IMapper _mapper;

    public TicketStatusManager(IGenericRepository<TicketStatus> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TicketStatusDto>> GetAllAsync()
    {
        var statuses = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TicketStatusDto>>(statuses);
    }
}
