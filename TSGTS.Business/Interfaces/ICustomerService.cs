using TSGTS.Core.DTOs;

namespace TSGTS.Business.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<IEnumerable<CustomerDto>> SearchAsync(string term);
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CustomerCreateDto dto);
    Task<CustomerDto?> UpdateAsync(int id, CustomerCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
