using FluentValidation.TestHelper;
using TradeRiskEvaluator.Application.Features.CalculateRiskDistribution;

namespace TradeRiskEvaluator.Application.Tests.Features.CalculateRiskDistribution
{
    public class CalculateRiskDistributionCommandValidatorTests
    {
        private readonly CalculateRiskDistributionCommandValidator _calculateRiskDistributionCommandValidator;

        public CalculateRiskDistributionCommandValidatorTests()
        {
            _calculateRiskDistributionCommandValidator = new CalculateRiskDistributionCommandValidator();
        }

        [Fact]
        public void Validate_WhenTradesListIsEmpty_ShouldNotHaveAnyValidationErrors()
        {
            var command = new CalculateRiskDistributionCommand
            {
                Trades = []
            };

            var result = _calculateRiskDistributionCommandValidator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenCommandContainsInvalidTrade_ShouldHaveValidationErrors()
        {
            var command = new CalculateRiskDistributionCommand
            {
                Trades = [new TradeDistributionRequest()]
            };

            var result = _calculateRiskDistributionCommandValidator.TestValidate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WhenCommandContainsValidTrade_ShouldNotHaveValidationErrors()
        {
            var validTrade = new TradeDistributionRequest
            {
                Value = 2000000,
                ClientSector = "Private",
                ClientId = "CLI001"
            };

            var command = new CalculateRiskDistributionCommand
            {
                Trades = [validTrade]
            };

            var result = _calculateRiskDistributionCommandValidator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}