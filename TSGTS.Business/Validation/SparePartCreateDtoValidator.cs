using FluentValidation;
using TSGTS.Core.DTOs;

namespace TSGTS.Business.Validation;

public class SparePartCreateDtoValidator : AbstractValidator<SparePartCreateDto>
{
    public SparePartCreateDtoValidator()
    {
        RuleFor(x => x.PartName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.PartCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CriticalLevel).GreaterThanOrEqualTo(0);
    }
}
