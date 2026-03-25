using TradeRiskEvaluator.Domain.Entities;

namespace TradeRiskEvaluator.Domain.RiskRules
{
    public interface IRiskRule
    {
        string Category { get; }
        bool IsMatch(Trade trade);
    }
}
