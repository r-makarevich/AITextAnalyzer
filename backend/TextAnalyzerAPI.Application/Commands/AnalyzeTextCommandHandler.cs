using MediatR;
using TextAnalyzerAPI.Application.Interfaces;
using TextAnalyzerAPI.Application.Models;
using TextAnalyzerAPI.Application.Services;
using TextAnalyzerAPI.Domain.Entities;
using TextAnalyzerAPI.Domain.Enums;

namespace TextAnalyzerAPI.Application.Commands;

public class AnalyzeTextCommandHandler : IRequestHandler<AnalyzeTextCommand, AnalyzeTextResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ISentimentAnalysisService _sentimentAnalysisService;
    private readonly ILanguageDetectionService _languageDetectionService;

    public AnalyzeTextCommandHandler(
        IApplicationDbContext context,
        ISentimentAnalysisService sentimentAnalysisService,
        ILanguageDetectionService languageDetectionService)
    {
        _context = context;
        _sentimentAnalysisService = sentimentAnalysisService;
        _languageDetectionService = languageDetectionService;
    }

    public async Task<AnalyzeTextResponse> Handle(AnalyzeTextCommand request, CancellationToken cancellationToken)
    {
        Sentiment sentiment = _sentimentAnalysisService.AnalyzeSentiment(request.Content);
        string language = _languageDetectionService.DetectLanguage(request.Content);

        TextAnalysis textAnalysis = new TextAnalysis
        {
            Content = request.Content,
            Sentiment = sentiment,
            Language = language,
            CreatedAt = DateTime.UtcNow
        };

        _context.TextAnalyses.Add(textAnalysis);
        await _context.SaveChangesAsync(cancellationToken);

        return new AnalyzeTextResponse
        {
            Id = textAnalysis.Id,
            Content = textAnalysis.Content,
            Sentiment = textAnalysis.Sentiment,
            SentimentText = textAnalysis.Sentiment.ToString(),
            Language = textAnalysis.Language,
            CreatedAt = textAnalysis.CreatedAt
        };
    }
}
