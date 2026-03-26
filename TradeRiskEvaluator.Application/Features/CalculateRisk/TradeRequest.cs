using FluentValidation;
using TradeRiskEvaluator.Application.Constants;
using TradeRiskEvaluator.Domain.Enum;

namespace TradeRiskEvaluator.Application.Features.CalculateRisk
{
    public class TradeRequest
    {
        public decimal Value { get; set; }
        public string ClientSector { get; set; } = string.Empty;
    }

    public class TradeRequestValidator : AbstractValidator<TradeRequest>
    {
        public TradeRequestValidator()
        {
            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage(ValidationMessages.GreaterThanZero);

            RuleFor(x => x.ClientSector)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField)
                .IsEnumName(typeof(Sector), caseSensitive: false)
                .WithMessage(ValidationMessages.InvalidSector);
        }
    }
}
