using TradeRiskEvaluator.Domain.Entities;

namespace TradeRiskEvaluator.Domain.RiskRules
{
    public class RiskEvaluator : IRiskEvaluator
    {
        private readonly IEnumerable<IRiskRule> _rules;

        public RiskEvaluator(IEnumerable<IRiskRule> rules)
        {
            _rules = rules;
        }

        public string Evaluate(Trade trade)
        {
            foreach (var rule in _rules)
            {
                if (rule.IsMatch(trade))
                    return rule.Category;
            }
            return "UNCLASSIFIED";
        }
    }
}