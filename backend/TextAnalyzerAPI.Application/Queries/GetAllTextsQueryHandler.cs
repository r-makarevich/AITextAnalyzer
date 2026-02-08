using MediatR;
using Microsoft.EntityFrameworkCore;
using TextAnalyzerAPI.Application.Interfaces;
using TextAnalyzerAPI.Domain.Entities;

namespace TextAnalyzerAPI.Application.Queries;

public class GetAllTextsQueryHandler : IRequestHandler<GetAllTextsQuery, List<TextAnalysis>>
{
    private readonly IApplicationDbContext _context;

    public GetAllTextsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TextAnalysis>> Handle(GetAllTextsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TextAnalyses.ToListAsync(cancellationToken);
    }
}
