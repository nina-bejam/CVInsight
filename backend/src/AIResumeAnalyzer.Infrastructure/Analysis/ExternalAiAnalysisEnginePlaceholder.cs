using AIResumeAnalyzer.Application.Interfaces;
using AIResumeAnalyzer.Domain.Models;

namespace AIResumeAnalyzer.Infrastructure.Analysis;

/// <summary>
/// Future hook for an external AI provider. Register in DependencyInjection to replace the rule-based engine.
/// </summary>
public sealed class ExternalAiAnalysisEnginePlaceholder : IResumeAnalysisEngine
{
    public ResumeAnalysis Analyze(string resumeText, string targetRole) =>
        throw new NotImplementedException(
            "Implement IResumeAnalysisEngine for your AI provider and register it in Infrastructure.DependencyInjection.");
}
