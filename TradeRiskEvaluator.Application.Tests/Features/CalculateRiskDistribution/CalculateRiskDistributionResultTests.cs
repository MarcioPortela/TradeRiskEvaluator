using TradeRiskEvaluator.Application.Features.CalculateRiskDistribution;

namespace TradeRiskEvaluator.Application.Tests.Features.CalculateRiskDistribution
{
    public class CalculateRiskDistributionResultTests
    {
        [Fact]
        public void CategoryDistribution_ShouldInitializeWithExpectedDefaults()
        {
            var category = new CategoryDistribution();

            Assert.Equal(0, category.Count);
            Assert.Equal(0m, category.TotalValue);
            Assert.Equal(string.Empty, category.TopClient);
        }

        [Fact]
        public void CategoryDistribution_ShouldSetPropertiesCorrectly()
        {
            var expectedCount = 5;
            var expectedValue = 50000m;
            var expectedClient = "CLI001";

            var category = new CategoryDistribution
            {
                Count = expectedCount,
                TotalValue = expectedValue,
                TopClient = expectedClient
            };

            Assert.Equal(expectedCount, category.Count);
            Assert.Equal(expectedValue, category.TotalValue);
            Assert.Equal(expectedClient, category.TopClient);
        }

        [Fact]
        public void CalculateRiskDistributionResult_ShouldInitializeWithEmptyCollections()
        {
            var result = new CalculateRiskDistributionResult();

            Assert.NotNull(result.Categories);
            Assert.Empty(result.Categories);

            Assert.NotNull(result.Distribution);
            Assert.Empty(result.Distribution);

            Assert.Equal(0, result.ProcessingTimeMs);
        }

        [Fact]
        public void CalculateRiskDistributionResult_ShouldSetPropertiesCorrectly()
        {
            var expectedCategories = new List<string> { "LOWRISK", "HIGHRISK" };
            var expectedDistribution = new Dictionary<string, CategoryDistribution>
            {
                { "LOWRISK", new CategoryDistribution { Count = 2, TotalValue = 1500, TopClient = "CLI002" } }
            };
            var expectedTime = 125L;

            var result = new CalculateRiskDistributionResult
            {
                Categories = expectedCategories,
                Distribution = expectedDistribution,
                ProcessingTimeMs = expectedTime
            };

            Assert.Equal(expectedCategories, result.Categories);
            Assert.Equal(expectedDistribution, result.Distribution);
            Assert.Equal(expectedTime, result.ProcessingTimeMs);
        }
    }
}