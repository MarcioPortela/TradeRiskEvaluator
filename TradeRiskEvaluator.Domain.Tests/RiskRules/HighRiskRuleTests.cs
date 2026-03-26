using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.Enum;
using TradeRiskEvaluator.Domain.RiskRules;

namespace TradeRiskEvaluator.Domain.Tests.RiskRules
{
    public class HighRiskRuleTests
    {
        private readonly HighRiskRule _rule;

        public HighRiskRuleTests()
        {
            _rule = new HighRiskRule();
        }

        [Fact]
        public void Category_ShouldReturn_HIGHRISK()
        {
            var category = _rule.Category;

            Assert.Equal("HIGHRISK", category);
        }

        [Theory]
        [InlineData(2000000, Sector.Private, true)]
        [InlineData(1000000.01, Sector.Private, true)]

        [InlineData(1000000, Sector.Private, false)]
        [InlineData(999999.99, Sector.Private, false)]
        [InlineData(500000, Sector.Private, false)]

        [InlineData(2000000, Sector.Public, false)]
        [InlineData(1000000.01, Sector.Public, false)]
        [InlineData(500000, Sector.Public, false)]
        public void IsMatch_ShouldEvaluateCorrectly_BasedOnTradeValueAndSector(double tradeValueDouble, Sector clientSector, bool expected)
        {
            decimal tradeValue = (decimal)tradeValueDouble;
            var trade = new Trade(tradeValue, clientSector);

            var result = _rule.IsMatch(trade);

            Assert.Equal(expected, result);
        }

    }
}
