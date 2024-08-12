using FluentValidation;

namespace Erm.BusinessLayer.Validators;

internal sealed class RiskDTOValidation : AbstractValidator<RiskDTO>
{
    internal RiskDTOValidation()
    {
        RuleFor(prop => prop.Type).NotEmpty().MinimumLength(3).MaximumLength(10);
        RuleFor(prop => prop.Description).NotEmpty().MinimumLength(15).MaximumLength(500);
        RuleFor(prop => prop.Probability).GreaterThanOrEqualTo(1).LessThanOrEqualTo(10);
        RuleFor(prop => prop.BusinessImpact).GreaterThanOrEqualTo(1).LessThanOrEqualTo(10);
        RuleFor(prop => prop.OccurenceData);
        RuleFor(prop => prop.OccurenceData).NotNull().WithMessage("Дата не должна быть пустой").Must(date => date.HasValue && date.Value < DateTime.Now)
            .WithMessage("Введите корректную дату");
    }
}