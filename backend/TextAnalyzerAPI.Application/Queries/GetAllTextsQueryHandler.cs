using MediatR;
using Microsoft.EntityFrameworkCore;
using TextAnalyzerAPI.Application.Interfaces;
using TextAnalyzerAPI.Application.Models;

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
        var textAnalyses = await _context.TextAnalyses.ToListAsync(cancellationToken);
        
        return textAnalyses.Select(ta => new AnalyzeTextResponse
        {
            Id = ta.Id,
            Content = ta.Content,
            Sentiment = ta.Sentiment,
            SentimentText = ta.Sentiment.ToString(),
            CreatedAt = ta.CreatedAt
        }).ToList();
    }
}
