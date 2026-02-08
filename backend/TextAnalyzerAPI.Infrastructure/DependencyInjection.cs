using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TextAnalyzerAPI.Application.Interfaces;
using TextAnalyzerAPI.Infrastructure.Data;

namespace TextAnalyzerAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString = null)
    {
        // Set database path to Infrastructure project's Data folder
        string dbPath = connectionString ?? Path.Combine(
            Path.GetDirectoryName(typeof(DependencyInjection).Assembly.Location)!,
            "..", "..", "..", "..",
            "TextAnalyzerAPI.Infrastructure", "Data", "TextAnalyzer.db");
        
        string fullConnectionString = $"Data Source={dbPath}";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(fullConnectionString));

        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
