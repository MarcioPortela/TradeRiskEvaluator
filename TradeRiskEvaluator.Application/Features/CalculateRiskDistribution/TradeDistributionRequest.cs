using FluentValidation;

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
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("Trade value must be greater than zero.");

            RuleFor(x => x.ClientSector)
                .NotEmpty().WithMessage("ClientSector is required.")
                .Must(s => s.Equals("Public", StringComparison.OrdinalIgnoreCase) ||
                           s.Equals("Private", StringComparison.OrdinalIgnoreCase))
                .WithMessage("ClientSector must be exactly 'Public' or 'Private'.");

            RuleFor(x => x.ClientId).NotEmpty().WithMessage("ClientId is required for the summary.");
        }
    }
}
