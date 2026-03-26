using Xunit;
using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.Enum;
using TradeRiskEvaluator.Domain.RiskRules;

namespace TradeRiskEvaluator.Domain.Tests.RiskRules
{
    public class LowRiskRuleTests
    {
        private readonly LowRiskRule _rule;

        public LowRiskRuleTests()
        {
            _rule = new LowRiskRule();
        }

        [Fact]
        public void Category_ShouldReturn_LOWRISK()
        {
            var category = _rule.Category;

            Assert.Equal("LOWRISK", category);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(500000, true)]
        [InlineData(999999.99, true)]
        [InlineData(1000000, false)]
        [InlineData(2000000, false)]
        public void IsMatch_ShouldEvaluateCorrectly_BasedOnTradeValue(double tradeValueDouble, bool expected)
        {
            decimal tradeValue = (decimal)tradeValueDouble;
            var trade = new Trade(tradeValue, Sector.Private);

            var result = _rule.IsMatch(trade);

            Assert.Equal(expected, result);
        }
    }
}