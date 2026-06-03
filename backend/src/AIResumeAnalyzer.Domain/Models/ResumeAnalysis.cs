namespace AIResumeAnalyzer.Domain.Models;

public sealed class ResumeAnalysis
{
    public string Summary { get; init; } = string.Empty;
    public IReadOnlyList<string> DetectedSkills { get; init; } = [];
    public IReadOnlyList<string> MissingSkills { get; init; } = [];
    public IReadOnlyList<string> Feedback { get; init; } = [];
    public string LinkedInHeadline { get; init; } = string.Empty;
    public IReadOnlyList<string> ImprovementSuggestions { get; init; } = [];
}
