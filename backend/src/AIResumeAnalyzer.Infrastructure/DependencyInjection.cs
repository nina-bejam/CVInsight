using AIResumeAnalyzer.Application.Interfaces;
using AIResumeAnalyzer.Infrastructure.Analysis;
using Microsoft.Extensions.DependencyInjection;

namespace AIResumeAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IResumeAnalysisEngine, RuleBasedResumeAnalysisEngine>();
        return services;
    }
}
