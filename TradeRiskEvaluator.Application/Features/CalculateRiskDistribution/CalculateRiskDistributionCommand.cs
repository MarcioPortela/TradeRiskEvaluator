using FluentValidation;
using MediatR;

namespace TradeRiskEvaluator.Application.Features.CalculateRiskDistribution
{
    public class CalculateRiskDistributionCommand : IRequest<CalculateRiskDistributionResult>
    {
        public List<TradeDistributionRequest> Trades { get; set; } = new();
    }

    public class CalculateRiskDistributionCommandValidator : AbstractValidator<CalculateRiskDistributionCommand>
    {
        public CalculateRiskDistributionCommandValidator()
        {
            RuleForEach(x => x.Trades).SetValidator(new TradeDistributionRequestValidator());
        }
    }
}
