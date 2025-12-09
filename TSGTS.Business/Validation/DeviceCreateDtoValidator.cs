using FluentValidation;
using TSGTS.Core.DTOs;

namespace TSGTS.Business.Validation;

public class DeviceCreateDtoValidator : AbstractValidator<DeviceCreateDto>
{
    public DeviceCreateDtoValidator()
    {
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.BrandId).GreaterThan(0);
        RuleFor(x => x.ModelId).GreaterThan(0);
    }
}
