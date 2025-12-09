using TSGTS.Core.DTOs;

namespace TSGTS.Business.Interfaces;

public interface IDeviceService
{
    Task<IEnumerable<DeviceDto>> GetAllAsync();
    Task<DeviceDto?> GetByIdAsync(int id);
    Task<DeviceDto> CreateAsync(DeviceCreateDto dto);
    Task<DeviceDto?> UpdateAsync(int id, DeviceCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
