using TSGTS.Core.DTOs;

namespace TSGTS.Business.Interfaces;

public interface ISparePartService
{
    Task<IEnumerable<SparePartDto>> GetAllAsync();
    Task<SparePartDto?> GetByIdAsync(int id);
    Task<SparePartDto> CreateAsync(SparePartCreateDto dto);
    Task<SparePartDto?> UpdateAsync(int id, SparePartCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
