using FluentValidation.TestHelper;
using TradeRiskEvaluator.Application.Constants;
using TradeRiskEvaluator.Application.Features.CalculateRiskDistribution;

namespace TradeRiskEvaluator.Application.Tests.Features.CalculateRiskDistribution
{
    public class TradeDistributionRequestValidatorTests
    {
        private readonly TradeDistributionRequestValidator _validator;

        public TradeDistributionRequestValidatorTests()
        {
            _validator = new TradeDistributionRequestValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5000)]
        public void ShouldHaveError_WhenValueIsZeroOrLess(double valueDouble)
        {
            var request = new TradeDistributionRequest
            {
                Value = (decimal)valueDouble,
                ClientSector = "Public",
                ClientId = "CLI001"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.Value)
                  .WithErrorMessage(ValidationMessages.GreaterThanZero.Replace("{PropertyName}", "Value"));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(5000000)]
        public void ShouldNotHaveError_WhenValueIsGreaterThanZero(double valueDouble)
        {
            var request = new TradeDistributionRequest
            {
                Value = (decimal)valueDouble,
                ClientSector = "Private",
                ClientId = "CLI002"
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldHaveError_WhenClientSectorIsEmpty(string sector)
        {
            var request = new TradeDistributionRequest
            {
                Value = 1000,
                ClientSector = sector,
                ClientId = "CLI003"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.ClientSector)
                  .WithErrorMessage(ValidationMessages.RequiredField.Replace("{PropertyName}", "Client Sector"));
        }

        [Theory]
        [InlineData("ABC")]
        [InlineData("123")]
        [InlineData("XYZ")]
        public void ShouldHaveError_WhenClientSectorIsNotPublicOrPrivate(string sector)
        {
            var request = new TradeDistributionRequest
            {
                Value = 1000,
                ClientSector = sector,
                ClientId = "CLI004"
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.ClientSector)
                  .WithErrorMessage(ValidationMessages.InvalidSector.Replace("{PropertyName}", "Client Sector"));
        }

        [Theory]
        [InlineData("Public")]
        [InlineData("Private")]
        [InlineData("public")]
        [InlineData("PRIVATE")]
        public void ShouldNotHaveError_WhenClientSectorIsValid(string sector)
        {
            var request = new TradeDistributionRequest
            {
                Value = 1000,
                ClientSector = sector,
                ClientId = "CLI005"
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ClientSector);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldHaveError_WhenClientIdIsEmptyOrNull(string clientId)
        {
            var request = new TradeDistributionRequest
            {
                Value = 1000,
                ClientSector = "Public",
                ClientId = clientId!
            };

            var result = _validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.ClientId)
                  .WithErrorMessage(ValidationMessages.RequiredField.Replace("{PropertyName}", "Client Id"));
        }

        [Fact]
        public void ShouldNotHaveError_WhenClientIdIsProvided()
        {
            var request = new TradeDistributionRequest
            {
                Value = 1000,
                ClientSector = "Public",
                ClientId = "CLI006"
            };

            var result = _validator.TestValidate(request);

            result.ShouldNotHaveValidationErrorFor(x => x.ClientId);
        }
    }
}