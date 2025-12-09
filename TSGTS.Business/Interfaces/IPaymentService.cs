using TSGTS.Core.DTOs;

namespace TSGTS.Business.Interfaces;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync();
    Task<PaymentDto?> GetByIdAsync(int id);
    Task<PaymentDto> CreateAsync(PaymentCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
