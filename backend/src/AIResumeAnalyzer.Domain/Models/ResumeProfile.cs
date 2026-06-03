namespace AIResumeAnalyzer.Domain.Models;

public sealed class ResumeProfile
{
    public string FullName { get; init; } = "Candidate";
    public string? Email { get; init; }
    public string? CurrentTitle { get; init; }
    public int YearsOfExperience { get; init; }
    public bool HasSkillsSection { get; init; }
    public bool HasSummarySection { get; init; }
    public bool HasExperienceSection { get; init; }
    public bool HasEducationSection { get; init; }
    public bool HasProjectsSection { get; init; }
    public int BulletPointCount { get; init; }
    public int QuantifiedBulletCount { get; init; }
    public IReadOnlyDictionary<string, int> SkillMentionCounts { get; init; } =
        new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
}
