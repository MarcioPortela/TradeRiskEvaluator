using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TradeRiskEvaluator.API.Controllers;
using TradeRiskEvaluator.Application.Features.CalculateRisk;
using TradeRiskEvaluator.Application.Features.CalculateRiskDistribution;

namespace TradeRiskEvaluator.API.Tests.Controllers
{
    public class TradesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TradesController _controller;

        public TradesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TradesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CalculateRisk_WhenCalledWithValidRequest_ShouldReturnOkResult()
        {
            var trades = new List<TradeRequest>
            {
                new() { Value = 2000000, ClientSector = "Private" },
                new() { Value = 400000, ClientSector = "Public" }
            };

            var expectedResult = new List<string> { "HIGHRISK", "MEDIUMRISK" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CalculateRiskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var response = await _controller.CalculateRisk(trades);

            var okResult = Assert.IsType<OkObjectResult>(response);
            var resultValue = Assert.IsAssignableFrom<List<string>>(okResult.Value);

            Assert.Equal(2, resultValue.Count);
            Assert.Equal(expectedResult, resultValue);

            _mediatorMock.Verify(m => m.Send(
                It.Is<CalculateRiskCommand>(c => c.Trades == trades),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CalculateRiskDistribution_WhenCalledWithValidRequest_ShouldReturnOkResult()
        {
            var trades = new List<TradeDistributionRequest>
            {
                new() { Value = 2000000, ClientSector = "Private", ClientId = "CLI001" },
                new() { Value = 400000, ClientSector = "Public", ClientId = "CLI002" }
            };

            var expectedResult = new CalculateRiskDistributionResult
            {
                Categories = new List<string> { "HIGHRISK", "MEDIUMRISK" },
                ProcessingTimeMs = 15,
                Distribution = new Dictionary<string, CategoryDistribution>
                {
                    { "HIGHRISK", new CategoryDistribution { Count = 1, TopClient = "CLI001", TotalValue = 2000000 } },
                    { "MEDIUMRISK", new CategoryDistribution { Count = 1, TopClient = "CLI002", TotalValue = 400000 } }
                }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CalculateRiskDistributionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var response = await _controller.CalculateRiskDistribution(trades);

            var okResult = Assert.IsType<OkObjectResult>(response);
            var resultValue = Assert.IsType<CalculateRiskDistributionResult>(okResult.Value);

            Assert.Equal(expectedResult.ProcessingTimeMs, resultValue.ProcessingTimeMs);
            Assert.Equal(expectedResult.Categories.Count, resultValue.Categories.Count);

            _mediatorMock.Verify(m => m.Send(
                It.Is<CalculateRiskDistributionCommand>(c => c.Trades == trades),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}