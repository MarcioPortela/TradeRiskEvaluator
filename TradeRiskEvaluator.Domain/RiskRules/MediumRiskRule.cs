using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.Enum;

namespace TradeRiskEvaluator.Domain.RiskRules
{
    public class MediumRiskRule : IRiskRule
    {
        public string Category => "MEDIUMRISK";

        public bool IsMatch(Trade trade) =>
            trade.Value > 1000000 && trade.ClientSector == Sector.Public;
    }
}
