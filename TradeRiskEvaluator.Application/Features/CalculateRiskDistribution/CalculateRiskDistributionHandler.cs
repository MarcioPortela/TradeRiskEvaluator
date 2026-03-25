using MediatR;
using System.Diagnostics;
using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.Enum;
using TradeRiskEvaluator.Domain.RiskRules;

namespace TradeRiskEvaluator.Application.Features.CalculateRiskDistribution
{
    public class CalculateRiskSummaryHandler : IRequestHandler<CalculateRiskDistributionCommand, CalculateRiskDistributionResult>
    {
        private readonly RiskEvaluator _evaluator;

        public CalculateRiskSummaryHandler(RiskEvaluator evaluator)
        {
            _evaluator = evaluator;
        }

        public Task<CalculateRiskDistributionResult> Handle(CalculateRiskDistributionCommand request, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var evaluatedTrades = request.Trades.AsParallel().Select(dto =>
            {
                var sectorEnum = Enum.Parse<Sector>(dto.ClientSector, ignoreCase: true);
                var tradeEntity = new Trade(dto.Value, sectorEnum, dto.ClientId);
                var riskCategory = _evaluator.Evaluate(tradeEntity);

                return new { Risk = riskCategory, Trade = tradeEntity };
            }).ToList();

            var summaryDictionary = new Dictionary<string, CategoryDistribution>();
            var uniqueCategories = evaluatedTrades.Select(x => x.Risk).Distinct().ToList();

            var groupedByRisk = evaluatedTrades.GroupBy(x => x.Risk);

            foreach (var riskGroup in groupedByRisk)
            {
                var topClient = riskGroup
                    .GroupBy(x => x.Trade.ClientId)
                    .OrderByDescending(clientGroup => clientGroup.Sum(x => x.Trade.Value))
                    .First()
                    .Key;

                summaryDictionary[riskGroup.Key] = new CategoryDistribution
                {
                    Count = riskGroup.Count(),
                    TotalValue = riskGroup.Sum(x => x.Trade.Value),
                    TopClient = topClient
                };
            }

            stopwatch.Stop();

            var result = new CalculateRiskDistributionResult
            {
                Categories = uniqueCategories,
                Distribution = summaryDictionary,
                ProcessingTimeMs = stopwatch.ElapsedMilliseconds
            };

            return Task.FromResult(result);
        }
    }
}
