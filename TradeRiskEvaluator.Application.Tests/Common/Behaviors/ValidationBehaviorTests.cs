using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using TradeRiskEvaluator.Application.Common.Behaviors;

namespace TradeRiskEvaluator.Application.Tests.Common.Behaviors
{
    public class MockRequest : IRequest<MockResponse> { }
    public class MockResponse { }

    public class ValidationBehaviorTests
    {
        [Fact]
        public async Task Handle_ShouldCallNext_WhenNoValidatorsAreProvided()
        {
            var validators = Enumerable.Empty<IValidator<MockRequest>>();
            var behavior = new ValidationBehavior<MockRequest, MockResponse>(validators);

            var request = new MockRequest();
            var nextMock = new Mock<RequestHandlerDelegate<MockResponse>>();
            nextMock.Setup(n => n.Invoke(It.IsAny<CancellationToken>())).ReturnsAsync(new MockResponse());

            var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

            Assert.NotNull(result);
            nextMock.Verify(n => n.Invoke(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallNext_WhenAllValidatorsPass()
        {
            var validatorMock = new Mock<IValidator<MockRequest>>();

            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<MockRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var validators = new List<IValidator<MockRequest>> { validatorMock.Object };
            var behavior = new ValidationBehavior<MockRequest, MockResponse>(validators);

            var request = new MockRequest();
            var nextMock = new Mock<RequestHandlerDelegate<MockResponse>>();
            nextMock.Setup(n => n.Invoke(It.IsAny<CancellationToken>())).ReturnsAsync(new MockResponse());

            var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

            Assert.NotNull(result);
            nextMock.Verify(n => n.Invoke(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidatorsFail()
        {
            var validatorMock = new Mock<IValidator<MockRequest>>();

            var validationFailure = new ValidationFailure("PropertyA", "PropertyA is invalid.");
            var validationResult = new ValidationResult(new[] { validationFailure });

            validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<MockRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var validators = new List<IValidator<MockRequest>> { validatorMock.Object };
            var behavior = new ValidationBehavior<MockRequest, MockResponse>(validators);

            var request = new MockRequest();
            var nextMock = new Mock<RequestHandlerDelegate<MockResponse>>();

            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                behavior.Handle(request, nextMock.Object, CancellationToken.None));

            Assert.Contains(exception.Errors, e => e.PropertyName == "PropertyA" && e.ErrorMessage == "PropertyA is invalid.");

            nextMock.Verify(n => n.Invoke(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}