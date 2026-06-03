namespace AIResumeAnalyzer.Application.DTOs;

public sealed class ResumeAnalysisResponseDto
{
    public string Summary { get; init; } = string.Empty;
    public IReadOnlyList<string> DetectedSkills { get; init; } = [];
    public IReadOnlyList<string> MissingSkills { get; init; } = [];
    public IReadOnlyList<string> Feedback { get; init; } = [];
    public string LinkedInHeadline { get; init; } = string.Empty;
    public IReadOnlyList<string> ImprovementSuggestions { get; init; } = [];
}
