using FluentValidation.TestHelper;
using TradeRiskEvaluator.Application.Features.CalculateRisk;

namespace TradeRiskEvaluator.Application.Tests.Features.CalculateRisk
{
    public class TradeRequestValidatorTests
    {
        private readonly TradeRequestValidator _validator;

        public TradeRequestValidatorTests()
        {
            _validator = new TradeRequestValidator();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(5000000)]
        public void ShouldNotHaveError_WhenValueIsGreaterThanZero(double valueDouble)
        {
            var request = new TradeRequest { Value = (decimal)valueDouble, ClientSector = "Public" };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5000)]
        public void ShouldHaveError_WhenValueIsZeroOrLess(double valueDouble)
        {
            var request = new TradeRequest { Value = (decimal)valueDouble, ClientSector = "Private" };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Value)
                  .WithErrorMessage("Trade value must be greater than zero.");
        }

        [Theory]
        [InlineData("Public")]
        [InlineData("Private")]
        [InlineData("public")]
        [InlineData("PRIVATE")]
        public void ShouldNotHaveError_WhenClientSectorIsValid(string sector)
        {
            var request = new TradeRequest { Value = 1000, ClientSector = sector };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ClientSector);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveError_WhenClientSectorIsEmptyOrNull(string sector)
        {
            var request = new TradeRequest { Value = 1000, ClientSector = sector };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.ClientSector)
                  .WithErrorMessage("ClientSector is required.");
        }

        [Theory]
        [InlineData("ABC")]
        [InlineData("123")]
        [InlineData(" ")]
        public void ShouldHaveError_WhenClientSectorIsNotPublicOrPrivate(string sector)
        {
            var request = new TradeRequest { Value = 1000, ClientSector = sector };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.ClientSector)
                  .WithErrorMessage("ClientSector must be exactly 'Public' or 'Private'.");
        }
    }
}