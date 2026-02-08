using MediatR;
using Microsoft.EntityFrameworkCore;
using TextAnalyzerAPI.Application.Interfaces;

namespace TextAnalyzerAPI.Application.Commands;

public class DeleteTextCommandHandler : IRequestHandler<DeleteTextCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteTextCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTextCommand request, CancellationToken cancellationToken)
    {
        var textAnalysis = await _context.TextAnalyses
            .FirstOrDefaultAsync(ta => ta.Id == request.Id, cancellationToken);

        if (textAnalysis == null)
        {
            return false;
        }

        _context.TextAnalyses.Remove(textAnalysis);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
