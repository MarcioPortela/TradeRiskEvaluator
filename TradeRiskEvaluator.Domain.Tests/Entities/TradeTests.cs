using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.Enum;

namespace TradeRiskEvaluator.Domain.Tests.Entities
{
    public class TradeTests
    {
        [Fact]
        public void Constructor_WithTwoParameters_ShouldSetPropertiesCorrectly()
        {
            decimal expectedValue = 1500000;
            var expectedSector = Sector.Private;

            var trade = new Trade(expectedValue, expectedSector);

            Assert.Equal(expectedValue, trade.Value);
            Assert.Equal(expectedSector, trade.ClientSector);
            Assert.Null(trade.ClientId);
        }

        [Fact]
        public void Constructor_WithThreeParameters_ShouldSetPropertiesCorrectly()
        {
            decimal expectedValue = 500000;
            var expectedSector = Sector.Public;
            string expectedClientId = "CLI001";

            var trade = new Trade(expectedValue, expectedSector, expectedClientId);

            Assert.Equal(expectedValue, trade.Value);
            Assert.Equal(expectedSector, trade.ClientSector);
            Assert.Equal(expectedClientId, trade.ClientId);
        }
    }
}