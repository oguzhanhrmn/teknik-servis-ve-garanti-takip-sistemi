using AutoMapper;
using TSGTS.Business.Interfaces;
using TSGTS.Core.DTOs;
using TSGTS.Core.Entities;
using TSGTS.DataAccess.Repositories;

namespace TSGTS.Business.Services;

public class DeviceManager : IDeviceService
{
    private readonly IGenericRepository<Device> _repository;
    private readonly IMapper _mapper;

    public DeviceManager(IGenericRepository<Device> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DeviceDto>> GetAllAsync()
    {
        var devices = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<DeviceDto>>(devices);
    }

    public async Task<DeviceDto?> GetByIdAsync(int id)
    {
        var device = await _repository.GetByIdAsync(id);
        return device is null ? null : _mapper.Map<DeviceDto>(device);
    }

    public async Task<DeviceDto> CreateAsync(DeviceCreateDto dto)
    {
        var entity = _mapper.Map<Device>(dto);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return _mapper.Map<DeviceDto>(entity);
    }

    public async Task<DeviceDto?> UpdateAsync(int id, DeviceCreateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return null;

        _mapper.Map(dto, existing);
        _repository.Update(existing);
        await _repository.SaveChangesAsync();
        return _mapper.Map<DeviceDto>(existing);
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
