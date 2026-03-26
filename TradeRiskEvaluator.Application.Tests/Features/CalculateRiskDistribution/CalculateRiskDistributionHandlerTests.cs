using Moq;
using TradeRiskEvaluator.Application.Features.CalculateRiskDistribution;
using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.RiskRules;

namespace TradeRiskEvaluator.Application.Tests.Features.CalculateRiskDistribution
{
    public class CalculateRiskDistributionHandlerTests
    {
        private readonly Mock<IRiskEvaluator> _riskEvaluatorMock;
        private readonly CalculateRiskDistributionHandler _handler;

        public CalculateRiskDistributionHandlerTests()
        {
            _riskEvaluatorMock = new Mock<IRiskEvaluator>();
            _handler = new CalculateRiskDistributionHandler(_riskEvaluatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCalculateCorrectDistributionAndTopClient()
        {
            var command = new CalculateRiskDistributionCommand
            {
                Trades = new List<TradeDistributionRequest>
                {
                    new() { Value = 1000, ClientSector = "Public", ClientId = "CLI001" },
                    new() { Value = 2000, ClientSector = "Public", ClientId = "CLI001" },
                    new() { Value = 4000, ClientSector = "Private", ClientId = "CLI002" }, 
                    new() { Value = 5000, ClientSector = "Public", ClientId = "CLI003" }
                }
            };

            _riskEvaluatorMock.Setup(x => x.Evaluate(It.Is<Trade>(t => t.Value <= 4000)))
                              .Returns("LOWRISK");

            _riskEvaluatorMock.Setup(x => x.Evaluate(It.Is<Trade>(t => t.Value > 4000)))
                              .Returns("HIGHRISK");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Categories.Count);
            Assert.Contains("LOWRISK", result.Categories);
            Assert.Contains("HIGHRISK", result.Categories);

            var lowRiskDistribution = result.Distribution["LOWRISK"];
            Assert.Equal(3, lowRiskDistribution.Count);
            Assert.Equal(7000m, lowRiskDistribution.TotalValue);
            Assert.Equal("CLI002", lowRiskDistribution.TopClient);

            var highRiskDistribution = result.Distribution["HIGHRISK"];
            Assert.Equal(1, highRiskDistribution.Count);
            Assert.Equal(5000m, highRiskDistribution.TotalValue);
            Assert.Equal("CLI003", highRiskDistribution.TopClient);

            Assert.True(result.ProcessingTimeMs >= 0);
        }

        [Fact]
        public async Task Handle_WhenTradesListIsEmpty_ShouldReturnEmptyResult()
        {
            var command = new CalculateRiskDistributionCommand
            {
                Trades = new List<TradeDistributionRequest>()
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result.Categories);
            Assert.Empty(result.Distribution);
            Assert.True(result.ProcessingTimeMs >= 0);

            _riskEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Trade>()), Times.Never);
        }
    }
}