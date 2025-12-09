using TSGTS.Core.DTOs;

namespace TSGTS.Business.Interfaces;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllAsync();
    Task<InvoiceDto?> GetByIdAsync(int id);
    Task<InvoiceDto> CreateAsync(InvoiceCreateDto dto);
    Task<InvoiceDto?> UpdateAsync(int id, InvoiceCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
