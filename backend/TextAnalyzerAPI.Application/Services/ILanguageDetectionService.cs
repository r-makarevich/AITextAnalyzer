namespace TextAnalyzerAPI.Application.Services;

public interface ILanguageDetectionService
{
    string DetectLanguage(string text);
}
