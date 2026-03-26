using FluentValidation;

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
                .GreaterThan(0).WithMessage("Trade value must be greater than zero.");

            RuleFor(x => x.ClientSector)
                .NotEmpty().WithMessage("ClientSector is required.")
                .Must(sector => sector != null && 
                                (sector.Equals("Public", StringComparison.OrdinalIgnoreCase) ||
                                sector.Equals("Private", StringComparison.OrdinalIgnoreCase)))
                .WithMessage("ClientSector must be exactly 'Public' or 'Private'.");
        }
    }

}
