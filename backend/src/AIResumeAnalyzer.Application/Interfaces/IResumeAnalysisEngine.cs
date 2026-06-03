using AIResumeAnalyzer.Domain.Models;

namespace AIResumeAnalyzer.Application.Interfaces;

/// <summary>
/// Pluggable analysis engine. Swap rule-based vs external AI without changing controllers.
/// </summary>
public interface IResumeAnalysisEngine
{
    ResumeAnalysis Analyze(string resumeText, string targetRole);
}
