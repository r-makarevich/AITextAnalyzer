using MediatR;
using TextAnalyzerAPI.Application.Interfaces;
using TextAnalyzerAPI.Domain.Entities;

namespace TextAnalyzerAPI.Application.Commands;

public class AnalyzeTextCommandHandler : IRequestHandler<AnalyzeTextCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AnalyzeTextCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(AnalyzeTextCommand request, CancellationToken cancellationToken)
    {
        var textAnalysis = new TextAnalysis
        {
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.TextAnalyses.Add(textAnalysis);
        await _context.SaveChangesAsync(cancellationToken);

        return textAnalysis.Id;
    }
}
