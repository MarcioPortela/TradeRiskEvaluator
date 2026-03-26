using Moq;
using TradeRiskEvaluator.Application.Features.CalculateRisk;
using TradeRiskEvaluator.Domain.Entities;
using TradeRiskEvaluator.Domain.RiskRules;

namespace TradeRiskEvaluator.Application.Tests.Features.CalculateRisk
{
    public class CalculateRiskHandlerTests
    {
        private readonly Mock<IRiskEvaluator> _riskEvaluatorMock;
        private readonly CalculateRiskHandler _calculateRiskHandler;

        public CalculateRiskHandlerTests()
        {
            _riskEvaluatorMock = new Mock<IRiskEvaluator>();
            _calculateRiskHandler = new CalculateRiskHandler(_riskEvaluatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnRiskCategories_ForValidTrades()
        {
            var command = new CalculateRiskCommand
            {
                Trades = [
                    new TradeRequest { Value = 2000000, ClientSector = "Private" },
                    new TradeRequest { Value = 400000, ClientSector = "Public" }
                ]
            };

            _riskEvaluatorMock.Setup(x => x.Evaluate(It.Is<Trade>(t => t.Value == 2000000)))
                          .Returns("HIGHRISK");

            _riskEvaluatorMock.Setup(x => x.Evaluate(It.Is<Trade>(t => t.Value == 400000)))
                          .Returns("LOWRISK");

            var result = await _calculateRiskHandler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("HIGHRISK", result[0]);
            Assert.Equal("LOWRISK", result[1]);
            
            _riskEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Trade>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoTradesAreProvided()
        {
            var command = new CalculateRiskCommand { Trades = [] };

            var result = await _calculateRiskHandler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
            _riskEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Trade>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenSectorIsInvalid()
        {
            var command = new CalculateRiskCommand
            {
                Trades = [
                    new TradeRequest { Value = 1000, ClientSector = "MockSector" }
                ]
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _calculateRiskHandler.Handle(command, CancellationToken.None));
        }
    }
}