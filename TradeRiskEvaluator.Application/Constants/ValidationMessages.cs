namespace TradeRiskEvaluator.Application.Constants
{
    public static class ValidationMessages
    {
        public const string GreaterThanZero = "The {PropertyName} must be greater than zero.";
        public const string RequiredField = "{PropertyName} is required.";
        public const string InvalidSector = "{PropertyName} must be exactly 'Public' or 'Private'.";
        public const string EmptyTradesList = "The list of trades cannot be empty.";
    }
}