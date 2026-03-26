using TradeRiskEvaluator.Domain.Entities;

namespace TradeRiskEvaluator.Domain.RiskRules
{
    public interface IRiskEvaluator
    {
        string Evaluate(Trade trade);
    }
}