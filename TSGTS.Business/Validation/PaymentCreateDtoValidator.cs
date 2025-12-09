using FluentValidation;
using TSGTS.Core.DTOs;

namespace TSGTS.Business.Validation;

public class PaymentCreateDtoValidator : AbstractValidator<PaymentCreateDto>
{
    public PaymentCreateDtoValidator()
    {
        RuleFor(x => x.InvoiceId).GreaterThan(0);
        RuleFor(x => x.PaymentTypeId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
    }
}
