using Microsoft.EntityFrameworkCore;
using TextAnalyzerAPI.Application.Interfaces;
using TextAnalyzerAPI.Domain.Entities;

namespace TextAnalyzerAPI.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TextAnalysis> TextAnalyses => Set<TextAnalysis>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TextAnalysis>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}
