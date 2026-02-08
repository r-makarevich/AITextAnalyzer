namespace TextAnalyzerAPI.Domain.Entities;

public class TextAnalysis
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
