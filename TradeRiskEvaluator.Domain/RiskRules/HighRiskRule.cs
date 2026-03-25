using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.Enum;

namespace TradeRiskEvaluator.Domain.RiskRules
{
    public class HighRiskRule : IRiskRule
    {
        public string Category => "HIGHRISK";

        public bool IsMatch(Trade trade) =>
            trade.Value > 1000000 && trade.ClientSector == Sector.Private;
    }
}
