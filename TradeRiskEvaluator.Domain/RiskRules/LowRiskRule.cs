using TradeRiskEvaluator.Domain.Entities;

namespace TradeRiskEvaluator.Domain.RiskRules
{
    public class LowRiskRule : IRiskRule
    {
        public string Category => "LOWRISK";

        public bool IsMatch(Trade trade) =>
            trade.Value < 1000000;
    }
}
