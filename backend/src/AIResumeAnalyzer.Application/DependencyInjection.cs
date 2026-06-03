using AIResumeAnalyzer.Application.Interfaces;
using AIResumeAnalyzer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AIResumeAnalyzer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IResumeAnalyzerService, ResumeAnalyzerService>();
        return services;
    }
}
