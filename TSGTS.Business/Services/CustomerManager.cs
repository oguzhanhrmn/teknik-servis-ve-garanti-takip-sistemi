using AutoMapper;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;
using TSGTS.DataAccess.Repositories;

namespace TSGTS.Business.Services;

public class CustomerManager : ICustomerService
{
    private readonly IGenericRepository<Customer> _repository;
    private readonly IMapper _mapper;

    public CustomerManager(IGenericRepository<Customer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<IEnumerable<CustomerDto>> SearchAsync(string term)
    {
        term = term?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(term))
            return await GetAllAsync();

        term = term.ToLowerInvariant();
        var filtered = await _repository.FindAsync(c =>
            c.FirstName.ToLower().Contains(term) ||
            c.LastName.ToLower().Contains(term) ||
            c.Phone.ToLower().Contains(term) ||
            c.Email.ToLower().Contains(term) ||
            c.TaxNo.ToLower().Contains(term));

        return _mapper.Map<IEnumerable<CustomerDto>>(filtered);
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer is null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> CreateAsync(CustomerCreateDto dto)
    {
        var phone = dto.Phone?.Trim();
        var lowerEmail = dto.Email?.Trim().ToLowerInvariant();

        // Aynı telefon veya e-posta ile kayıtlı müşteri varsa tekrar ekleme.
        var existing = (await _repository.FindAsync(c =>
            (phone != null && c.Phone == phone) ||
            (lowerEmail != null && c.Email.ToLower() == lowerEmail)))
            .FirstOrDefault();
        if (existing is not null)
            return _mapper.Map<CustomerDto>(existing);

        var entity = _mapper.Map<Customer>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<CustomerDto>(entity);
    }

    public async Task<CustomerDto?> UpdateAsync(int id, CustomerCreateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return null;

        _mapper.Map(dto, existing);
        _repository.Update(existing);
        await _repository.SaveChangesAsync();
        return _mapper.Map<CustomerDto>(existing);
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
