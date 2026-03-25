namespace TradeRiskEvaluator.Application.Features.CalculateRiskDistribution
{
    public class CategoryDistribution
    {
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
        public string TopClient { get; set; } = string.Empty;
    }

    public class CalculateRiskDistributionResult
    {
        public List<string> Categories { get; set; } = new();
        public Dictionary<string, CategoryDistribution> Distribution { get; set; } = new();
        public long ProcessingTimeMs { get; set; }
    }
}
