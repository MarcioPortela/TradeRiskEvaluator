using FluentValidation;
using MediatR;

namespace TradeRiskEvaluator.Application.Features.CalculateRisk
{
    public class CalculateRiskCommand : IRequest<List<string>>
    {
        public List<TradeRequest> Trades { get; set; } = new();
    }

    public class CalculateRiskCommandValidator : AbstractValidator<CalculateRiskCommand>
    {
        public CalculateRiskCommandValidator()
        {
            RuleFor(x => x.Trades)
                .NotEmpty()
                .WithMessage("The list of trades cannot be empty.");

            RuleForEach(x => x.Trades)
                .SetValidator(new TradeRequestValidator());
        }
    }
}
