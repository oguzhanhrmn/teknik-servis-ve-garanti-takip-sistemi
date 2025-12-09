using FluentValidation;
using TSGTS.Core.DTOs;

namespace TSGTS.Business.Validation;

public class ServiceTicketCreateDtoValidator : AbstractValidator<ServiceTicketCreateDto>
{
    public ServiceTicketCreateDtoValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.DeviceId).GreaterThan(0);
        RuleFor(x => x.OpenedByUserId).GreaterThan(0);
        RuleFor(x => x.StatusId).GreaterThan(0);
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}
