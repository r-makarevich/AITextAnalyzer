using TextAnalyzerAPI.Domain.Enums;

namespace TextAnalyzerAPI.Application.Models;

public class AnalyzeTextResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Sentiment Sentiment { get; set; }
    public string SentimentText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
