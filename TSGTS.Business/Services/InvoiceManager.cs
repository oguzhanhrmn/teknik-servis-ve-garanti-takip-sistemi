using AutoMapper;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;
using TSGTS.DataAccess.Repositories;

namespace TSGTS.Business.Services;

public class InvoiceManager : IInvoiceService
{
    private readonly IGenericRepository<Invoice> _repository;
    private readonly IMapper _mapper;

    public InvoiceManager(IGenericRepository<Invoice> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
    {
        var invoices = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
    }

    public async Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var inv = await _repository.GetByIdAsync(id);
        return inv is null ? null : _mapper.Map<InvoiceDto>(inv);
    }

    public async Task<InvoiceDto> CreateAsync(InvoiceCreateDto dto)
    {
        var entity = _mapper.Map<Invoice>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<InvoiceDto>(entity);
    }

    public async Task<InvoiceDto?> UpdateAsync(int id, InvoiceCreateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;
        _mapper.Map(dto, existing);
        _repository.Update(existing);
        await _repository.SaveChangesAsync();
        return _mapper.Map<InvoiceDto>(existing);
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
