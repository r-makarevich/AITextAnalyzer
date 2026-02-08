using TextAnalyzerAPI.Domain.Enums;

namespace TextAnalyzerAPI.Domain.Entities;

public class TextAnalysis
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Sentiment Sentiment { get; set; } = Sentiment.Neutral;
    public DateTime CreatedAt { get; set; }
}
