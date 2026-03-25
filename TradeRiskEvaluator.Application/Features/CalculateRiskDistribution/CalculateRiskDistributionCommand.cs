using FluentValidation;
using MediatR;

namespace TradeRiskEvaluator.Application.Features.CalculateRiskDistribution
{
    public class CalculateRiskDistributionCommand : IRequest<CalculateRiskDistributionResult>
    {
        public List<TradeDistributionRequest> Trades { get; set; } = new();
    }

    public class CalculateRiskSummaryCommandValidator : AbstractValidator<CalculateRiskDistributionCommand>
    {
        public CalculateRiskSummaryCommandValidator()
        {
            RuleForEach(x => x.Trades).SetValidator(new TradeDistributionRequestValidator());
        }
    }
}
