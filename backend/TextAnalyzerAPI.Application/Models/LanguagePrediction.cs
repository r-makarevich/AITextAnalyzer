using Microsoft.ML.Data;

namespace TextAnalyzerAPI.Application.Models;

public class LanguagePrediction
{
    [ColumnName("PredictedLabel")]
    public string Language { get; set; } = string.Empty;
}
