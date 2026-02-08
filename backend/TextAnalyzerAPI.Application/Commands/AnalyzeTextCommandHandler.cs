using MediatR;
using TextAnalyzerAPI.Application.Interfaces;
using TextAnalyzerAPI.Application.Models;
using TextAnalyzerAPI.Application.Services;
using TextAnalyzerAPI.Domain.Entities;

namespace TextAnalyzerAPI.Application.Commands;

public class AnalyzeTextCommandHandler : IRequestHandler<AnalyzeTextCommand, AnalyzeTextResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ISentimentAnalysisService _sentimentAnalysisService;

    public AnalyzeTextCommandHandler(
        IApplicationDbContext context,
        ISentimentAnalysisService sentimentAnalysisService)
    {
        _context = context;
        _sentimentAnalysisService = sentimentAnalysisService;
    }

    public async Task<AnalyzeTextResponse> Handle(AnalyzeTextCommand request, CancellationToken cancellationToken)
    {
        var sentiment = _sentimentAnalysisService.AnalyzeSentiment(request.Content);

        var textAnalysis = new TextAnalysis
        {
            Content = request.Content,
            Sentiment = sentiment,
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
            CreatedAt = textAnalysis.CreatedAt
        };
    }
}
