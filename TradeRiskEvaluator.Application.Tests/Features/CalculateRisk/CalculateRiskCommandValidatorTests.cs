using FluentValidation.TestHelper;
using TradeRiskEvaluator.Application.Features.CalculateRisk;

namespace TradeRiskEvaluator.Application.Tests.Features.CalculateRisk
{
    public class CalculateRiskCommandValidatorTests
    {
        private readonly CalculateRiskCommandValidator _validator;

        public CalculateRiskCommandValidatorTests()
        {
            _validator = new CalculateRiskCommandValidator();
        }

        [Fact]
        public void ShouldHaveError_WhenTradesListIsEmpty()
        {
            var command = new CalculateRiskCommand { Trades = [] };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Trades)
                  .WithErrorMessage("The list of trades cannot be empty.");
        }

        [Fact]
        public void ShouldHaveError_WhenTradesListIsNull()
        {
            var command = new CalculateRiskCommand { Trades = null! };

            var result = _validator.TestValidate(command);

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

            var result = _validator.TestValidate(command);

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

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor("Trades[1].Value")
                  .WithErrorMessage("Trade value must be greater than zero.");
        }
    }
}