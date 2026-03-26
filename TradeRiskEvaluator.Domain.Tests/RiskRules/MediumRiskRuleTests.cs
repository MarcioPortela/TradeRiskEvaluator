using Xunit;
using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.Enum;
using TradeRiskEvaluator.Domain.RiskRules;

namespace TradeRiskEvaluator.Domain.Tests.RiskRules
{
    public class MediumRiskRuleTests
    {
        private readonly MediumRiskRule _rule;

        public MediumRiskRuleTests()
        {
            _rule = new MediumRiskRule();
        }

        [Fact]
        public void Category_ShouldReturn_MEDIUMRISK()
        {
            var category = _rule.Category;

            Assert.Equal("MEDIUMRISK", category);
        }

        [Theory]
        [InlineData(2000000, Sector.Public, true)]
        [InlineData(1000000.01, Sector.Public, true)]

        [InlineData(1000000, Sector.Public, false)]
        [InlineData(999999.99, Sector.Public, false)]
        [InlineData(500000, Sector.Public, false)]

        [InlineData(2000000, Sector.Private, false)]
        [InlineData(1000000.01, Sector.Private, false)]
        [InlineData(500000, Sector.Private, false)]
        public void IsMatch_ShouldEvaluateCorrectly_BasedOnTradeValueAndSector(double tradeValueDouble, Sector clientSector, bool expected)
        {
            decimal tradeValue = (decimal)tradeValueDouble;
            var trade = new Trade(tradeValue, clientSector);

            var result = _rule.IsMatch(trade);

            Assert.Equal(expected, result);
        }
    }
}