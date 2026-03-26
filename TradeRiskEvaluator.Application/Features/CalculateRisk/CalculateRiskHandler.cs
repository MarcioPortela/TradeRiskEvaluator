using MediatR;
using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.Enum;
using TradeRiskEvaluator.Domain.RiskRules;

namespace TradeRiskEvaluator.Application.Features.CalculateRisk
{
    public class CalculateRiskHandler : IRequestHandler<CalculateRiskCommand, List<string>>
    {
        private readonly IRiskEvaluator _evaluator;

        public CalculateRiskHandler(IRiskEvaluator evaluator)
        {
            _evaluator = evaluator;
        }

        public Task<List<string>> Handle(CalculateRiskCommand request, CancellationToken cancellationToken)
        {
            var results = new List<string>();

            foreach (var trade in request.Trades)
            {
                var sectorEnum = Enum.Parse<Sector>(trade.ClientSector, ignoreCase: true);
                var tradeEntity = new Trade(trade.Value, sectorEnum);
                var riskCategory = _evaluator.Evaluate(tradeEntity);

                results.Add(riskCategory);
            }

            return Task.FromResult(results);
        }
    }
}
