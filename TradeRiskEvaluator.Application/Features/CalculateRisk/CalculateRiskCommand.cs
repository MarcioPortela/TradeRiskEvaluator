using FluentValidation;
using MediatR;
using TradeRiskEvaluator.Application.Constants;

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
                .WithMessage(ValidationMessages.EmptyTradesList);

            RuleForEach(x => x.Trades)
                .SetValidator(new TradeRequestValidator());
        }
    }
}
