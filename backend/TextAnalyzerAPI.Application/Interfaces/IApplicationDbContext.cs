using Microsoft.EntityFrameworkCore;
using TextAnalyzerAPI.Domain.Entities;

namespace TextAnalyzerAPI.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TextAnalysis> TextAnalyses { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
