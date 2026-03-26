using FluentValidation.TestHelper;
using TradeRiskEvaluator.Application.Constants;
using TradeRiskEvaluator.Application.Features.CalculateRisk;

namespace TradeRiskEvaluator.Application.Tests.Features.CalculateRisk
{
    public class TradeRequestValidatorTests
    {
        private readonly TradeRequestValidator _tradeRequestValidator;

        public TradeRequestValidatorTests()
        {
            _tradeRequestValidator = new TradeRequestValidator();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(5000000)]
        public void ShouldNotHaveError_WhenValueIsGreaterThanZero(double valueDouble)
        {
            var request = new TradeRequest { Value = (decimal)valueDouble, ClientSector = "Public" };

            var result = _tradeRequestValidator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5000)]
        public void ShouldHaveError_WhenValueIsZeroOrLess(double valueDouble)
        {
            var request = new TradeRequest { Value = (decimal)valueDouble, ClientSector = "Private" };

            var result = _tradeRequestValidator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Value)
                  .WithErrorMessage(ValidationMessages.GreaterThanZero.Replace("{PropertyName}", "Value"));
        }

        [Theory]
        [InlineData("Public")]
        [InlineData("Private")]
        [InlineData("public")]
        [InlineData("PRIVATE")]
        public void ShouldNotHaveError_WhenClientSectorIsValid(string sector)
        {
            var request = new TradeRequest { Value = 1000, ClientSector = sector };

            var result = _tradeRequestValidator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ClientSector);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveError_WhenClientSectorIsEmptyOrNull(string sector)
        {
            var request = new TradeRequest { Value = 1000, ClientSector = sector };

            var result = _tradeRequestValidator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.ClientSector)
                  .WithErrorMessage(ValidationMessages.RequiredField.Replace("{PropertyName}", "Client Sector"));
        }

        [Theory]
        [InlineData("ABC")]
        [InlineData("123")]
        [InlineData(" ")]
        public void ShouldHaveError_WhenClientSectorIsNotPublicOrPrivate(string sector)
        {
            var request = new TradeRequest { Value = 1000, ClientSector = sector };

            var result = _tradeRequestValidator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.ClientSector)
                  .WithErrorMessage(ValidationMessages.InvalidSector.Replace("{PropertyName}", "Client Sector"));
        }
    }
}