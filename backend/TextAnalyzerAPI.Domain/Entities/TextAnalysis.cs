using TextAnalyzerAPI.Domain.Enums;

namespace TextAnalyzerAPI.Domain.Entities;

public class TextAnalysis
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Sentiment Sentiment { get; set; } = Sentiment.Neutral;
    public string Language { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
