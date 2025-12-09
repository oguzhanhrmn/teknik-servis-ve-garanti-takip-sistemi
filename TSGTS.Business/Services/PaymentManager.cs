using AutoMapper;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;
using TSGTS.DataAccess.Repositories;

namespace TSGTS.Business.Services;

public class PaymentManager : IPaymentService
{
    private readonly IGenericRepository<Payment> _repository;
    private readonly IMapper _mapper;

    public PaymentManager(IGenericRepository<Payment> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync()
    {
        var payments = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<PaymentDto>>(payments);
    }

    public async Task<PaymentDto?> GetByIdAsync(int id)
    {
        var pay = await _repository.GetByIdAsync(id);
        return pay is null ? null : _mapper.Map<PaymentDto>(pay);
    }

    public async Task<PaymentDto> CreateAsync(PaymentCreateDto dto)
    {
        var entity = _mapper.Map<Payment>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<PaymentDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return false;
        _repository.Delete(existing);
        await _repository.SaveChangesAsync();
        return true;
    }
}
