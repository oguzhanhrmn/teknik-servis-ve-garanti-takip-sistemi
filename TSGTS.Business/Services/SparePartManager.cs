using AutoMapper;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;
using TSGTS.DataAccess.Repositories;

namespace TSGTS.Business.Services;

public class SparePartManager : ISparePartService
{
    private readonly IGenericRepository<SparePart> _repository;
    private readonly IMapper _mapper;

    public SparePartManager(IGenericRepository<SparePart> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SparePartDto>> GetAllAsync()
    {
        var parts = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<SparePartDto>>(parts);
    }

    public async Task<SparePartDto?> GetByIdAsync(int id)
    {
        var part = await _repository.GetByIdAsync(id);
        return part is null ? null : _mapper.Map<SparePartDto>(part);
    }

    public async Task<SparePartDto> CreateAsync(SparePartCreateDto dto)
    {
        var entity = _mapper.Map<SparePart>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<SparePartDto>(entity);
    }

    public async Task<SparePartDto?> UpdateAsync(int id, SparePartCreateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        _mapper.Map(dto, existing);
        _repository.Update(existing);
        await _repository.SaveChangesAsync();
        return _mapper.Map<SparePartDto>(existing);
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
