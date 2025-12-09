using TSGTS.Core.DTOs;

namespace TSGTS.Business.Interfaces;

public interface IPaymentTypeService
{
    Task<IEnumerable<PaymentTypeDto>> GetAllAsync();
}
