using MediatR;
using Microsoft.EntityFrameworkCore;
using TextAnalyzerAPI.Application.Interfaces;
using TextAnalyzerAPI.Application.Models;
using TextAnalyzerAPI.Domain.Entities;

namespace TextAnalyzerAPI.Application.Queries;

public class GetAllTextsQueryHandler : IRequestHandler<GetAllTextsQuery, List<AnalyzeTextResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllTextsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AnalyzeTextResponse>> Handle(GetAllTextsQuery request, CancellationToken cancellationToken)
    {
        List<TextAnalysis> textAnalyses = await _context.TextAnalyses.ToListAsync(cancellationToken);
        
        return textAnalyses.Select(ta => new AnalyzeTextResponse
        {
            Id = ta.Id,
            Content = ta.Content,
            Sentiment = ta.Sentiment,
            SentimentText = ta.Sentiment.ToString(),
            Language = ta.Language,
            CreatedAt = ta.CreatedAt
        }).ToList();
    }
}
