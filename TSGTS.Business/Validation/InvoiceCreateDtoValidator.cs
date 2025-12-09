using FluentValidation;
using TSGTS.Core.DTOs;

namespace TSGTS.Business.Validation;

public class InvoiceCreateDtoValidator : AbstractValidator<InvoiceCreateDto>
{
    public InvoiceCreateDtoValidator()
    {
        RuleFor(x => x.TicketId).GreaterThan(0);
        RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Discount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TaxAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.FinalAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.InvoiceDate).NotEmpty();
    }
}
