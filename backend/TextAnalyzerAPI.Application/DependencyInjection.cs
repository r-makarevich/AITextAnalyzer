using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TextAnalyzerAPI.Application.Services;

namespace TextAnalyzerAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();
        services.AddScoped<ILanguageDetectionService, LanguageDetectionService>();

        return services;
    }
}
