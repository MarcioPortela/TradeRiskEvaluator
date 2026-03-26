using FluentValidation;
using TradeRiskEvaluator.Application.Constants;
using TradeRiskEvaluator.Domain.Enum;

namespace TradeRiskEvaluator.Application.Features.CalculateRiskDistribution
{
    public class TradeDistributionRequest
    {
        public decimal Value { get; set; }
        public string ClientSector { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
    }

    public class TradeDistributionRequestValidator : AbstractValidator<TradeDistributionRequest>
    {
        public TradeDistributionRequestValidator()
        {
            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage(ValidationMessages.GreaterThanZero);

            RuleFor(x => x.ClientSector)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField)
                .IsEnumName(typeof(Sector), caseSensitive: false)
                .WithMessage(ValidationMessages.InvalidSector);

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField);
        }
    }
}
