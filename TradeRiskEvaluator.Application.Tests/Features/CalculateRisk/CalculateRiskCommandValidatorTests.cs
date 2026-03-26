using FluentValidation.TestHelper;
using TradeRiskEvaluator.Application.Constants;
using TradeRiskEvaluator.Application.Features.CalculateRisk;

namespace TradeRiskEvaluator.Application.Tests.Features.CalculateRisk
{
    public class CalculateRiskCommandValidatorTests
    {
        private readonly CalculateRiskCommandValidator _calculateRiskCommandValidator;

        public CalculateRiskCommandValidatorTests()
        {
            _calculateRiskCommandValidator = new CalculateRiskCommandValidator();
        }

        [Fact]
        public void ShouldHaveError_WhenTradesListIsEmpty()
        {
            var command = new CalculateRiskCommand { Trades = [] };

            var result = _calculateRiskCommandValidator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Trades)
                  .WithErrorMessage("The list of trades cannot be empty.");
        }

        [Fact]
        public void ShouldHaveError_WhenTradesListIsNull()
        {
            var command = new CalculateRiskCommand { Trades = null! };

            var result = _calculateRiskCommandValidator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Trades)
                  .WithErrorMessage("The list of trades cannot be empty.");
        }

        [Fact]
        public void ShouldNotHaveError_WhenTradesListIsValid()
        {
            var command = new CalculateRiskCommand
            {
                Trades = [
                    new TradeRequest { Value = 2000000, ClientSector = "Private" },
                    new TradeRequest { Value = 400000, ClientSector = "Public" }
                ]
            };

            var result = _calculateRiskCommandValidator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ShouldHaveError_WhenAnyTradeIsInvalid()
        {
            var command = new CalculateRiskCommand
            {
                Trades = [
                    new TradeRequest { Value = 2000000, ClientSector = "Private" },
                    new TradeRequest { Value = 0, ClientSector = "Public" }
                ]
            };

            var result = _calculateRiskCommandValidator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("Trades[1].Value")
                  .WithErrorMessage(ValidationMessages.GreaterThanZero.Replace("{PropertyName}", "Value"));
        }
    }
}