using Moq;
using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.Enum;
using TradeRiskEvaluator.Domain.RiskRules;

namespace TradeRiskEvaluator.Domain.Tests.RiskRules
{
    public class RiskEvaluatorTests
    {
        [Fact]
        public void Evaluate_ShouldReturnCategory_WhenSingleRuleMatches()
        {
            var mockRule = new Mock<IRiskRule>();
            mockRule.Setup(r => r.IsMatch(It.IsAny<Trade>())).Returns(true);
            mockRule.Setup(r => r.Category).Returns("MOCKRISK");

            var rules = new List<IRiskRule> { mockRule.Object };
            var evaluator = new RiskEvaluator(rules);
            var trade = new Trade(500000, Sector.Public);

            var result = evaluator.Evaluate(trade);

            Assert.Equal("MOCKRISK", result);
        }

        [Fact]
        public void Evaluate_ShouldReturnUNCLASSIFIED_WhenNoRuleMatches()
        {
            var mockRule = new Mock<IRiskRule>();
            mockRule.Setup(r => r.IsMatch(It.IsAny<Trade>())).Returns(false);

            var rules = new List<IRiskRule> { mockRule.Object };
            var evaluator = new RiskEvaluator(rules);
            var trade = new Trade(500000, Sector.Public);

            var result = evaluator.Evaluate(trade);

            Assert.Equal("UNCLASSIFIED", result);
        }

        [Fact]
        public void Evaluate_ShouldReturnFirstMatchingCategory_WhenMultipleRulesMatch()
        {
            var mockRule1 = new Mock<IRiskRule>();
            mockRule1.Setup(r => r.IsMatch(It.IsAny<Trade>())).Returns(true);
            mockRule1.Setup(r => r.Category).Returns("FIRSTRISK");

            var mockRule2 = new Mock<IRiskRule>();
            mockRule2.Setup(r => r.IsMatch(It.IsAny<Trade>())).Returns(true);
            mockRule2.Setup(r => r.Category).Returns("SECONDRISK");

            var rules = new List<IRiskRule> { mockRule1.Object, mockRule2.Object };
            var evaluator = new RiskEvaluator(rules);
            var trade = new Trade(500000, Sector.Public);

            var result = evaluator.Evaluate(trade);

            Assert.Equal("FIRSTRISK", result);
        }

        [Fact]
        public void Evaluate_ShouldReturnUNCLASSIFIED_WhenRuleListIsEmpty()
        {
            var rules = new List<IRiskRule>();
            var evaluator = new RiskEvaluator(rules);
            var trade = new Trade(500000, Sector.Public);

            var result = evaluator.Evaluate(trade);

            Assert.Equal("UNCLASSIFIED", result);
        }
    }
}