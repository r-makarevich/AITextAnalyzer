using TextAnalyzerAPI.Domain.Enums;

namespace TextAnalyzerAPI.Application.Services;

public interface ISentimentAnalysisService
{
    Sentiment AnalyzeSentiment(string text);
}
