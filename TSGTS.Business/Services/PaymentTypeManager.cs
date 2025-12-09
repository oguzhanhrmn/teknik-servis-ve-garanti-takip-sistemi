using AutoMapper;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;
using TSGTS.DataAccess.Repositories;

namespace TSGTS.Business.Services;

public class PaymentTypeManager : IPaymentTypeService
{
    private readonly IGenericRepository<PaymentType> _repository;
    private readonly IMapper _mapper;

    public PaymentTypeManager(IGenericRepository<PaymentType> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PaymentTypeDto>> GetAllAsync()
    {
        var types = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<PaymentTypeDto>>(types);
    }
}
