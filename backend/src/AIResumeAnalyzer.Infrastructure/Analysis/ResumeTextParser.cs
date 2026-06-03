using System.Text.RegularExpressions;
using AIResumeAnalyzer.Domain.Models;

namespace AIResumeAnalyzer.Infrastructure.Analysis;

internal static partial class ResumeTextParser
{
    public static ResumeProfile Parse(string resumeText)
    {
        var text = resumeText.Trim();
        var lines = text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var lower = text.ToLowerInvariant();

        return new ResumeProfile
        {
            FullName = ExtractName(lines),
            Email = ExtractEmail(text),
            CurrentTitle = ExtractTitle(lines),
            YearsOfExperience = EstimateYears(text),
            HasSkillsSection = SectionExists(lower, "skills", "technical skills", "technologies"),
            HasSummarySection = SectionExists(lower, "summary", "profile", "about"),
            HasExperienceSection = SectionExists(lower, "experience", "employment", "work history"),
            HasEducationSection = SectionExists(lower, "education", "academic"),
            HasProjectsSection = SectionExists(lower, "projects", "portfolio"),
            BulletPointCount = CountBullets(lines),
            QuantifiedBulletCount = CountQuantifiedBullets(lines),
            SkillMentionCounts = CountSkillMentions(text)
        };
    }

    private static string ExtractName(IReadOnlyList<string> lines)
    {
        if (lines.Count == 0) return "Candidate";
        var first = lines[0].Trim();
        if (first.Length > 60 || first.Contains('@')) return "Candidate";
        return first;
    }

    private static string? ExtractEmail(string text)
    {
        var match = EmailRegex().Match(text);
        return match.Success ? match.Value : null;
    }

    private static string? ExtractTitle(IReadOnlyList<string> lines)
    {
        var keywords = new[] { "engineer", "developer", "analyst", "manager", "architect", "consultant", "designer", "lead" };
        return lines.Skip(1).Take(6)
            .FirstOrDefault(l => keywords.Any(k => l.Contains(k, StringComparison.OrdinalIgnoreCase)));
    }

    private static int EstimateYears(string text)
    {
        var ranges = YearRangeRegex().Matches(text);
        if (ranges.Count == 0) return 0;

        var total = 0;
        foreach (Match match in ranges)
        {
            var start = int.Parse(match.Groups[1].Value);
            var endToken = match.Groups[2].Value;
            var end = endToken.Contains("present", StringComparison.OrdinalIgnoreCase)
                ? DateTime.UtcNow.Year
                : int.Parse(endToken);
            var span = end - start;
            if (span is >= 0 and <= 40)
                total += span;
        }

        return total;
    }

    private static bool SectionExists(string lower, params string[] headers) =>
        headers.Any(h => lower.Contains(h, StringComparison.Ordinal));

    private static int CountBullets(IEnumerable<string> lines) =>
        lines.Count(l =>
        {
            var t = l.TrimStart();
            return t.StartsWith('-') || t.StartsWith('•') || t.StartsWith('*');
        });

    private static int CountQuantifiedBullets(IEnumerable<string> lines) =>
        lines.Count(l =>
        {
            var t = l.TrimStart();
            var isBullet = t.StartsWith('-') || t.StartsWith('•') || t.StartsWith('*');
            return isBullet && (t.Contains('%') || QuantifiedRegex().IsMatch(t));
        });

    private static Dictionary<string, int> CountSkillMentions(string text)
    {
        var lower = text.ToLowerInvariant();
        var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var (skill, keywords) in SkillDictionary.TechnicalSkills)
        {
            var mentionCount = keywords.Sum(kw => CountOccurrences(lower, kw));
            if (mentionCount > 0)
                counts[skill] = mentionCount;
        }

        return counts;
    }

    private static int CountOccurrences(string text, string keyword)
    {
        var count = 0;
        var index = 0;
        while ((index = text.IndexOf(keyword, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += keyword.Length;
        }

        return count;
    }

    [GeneratedRegex(@"[\w.+-]+@[\w-]+\.[\w.-]+", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"(19|20)\d{2}\s*[-–—]\s*((19|20)\d{2}|present|current)", RegexOptions.IgnoreCase)]
    private static partial Regex YearRangeRegex();

    [GeneratedRegex(@"\d+\s*%|\$\s*\d|£\s*\d|\d+\s*(users|customers|requests|ms|seconds|minutes|hours|days|weeks|months)")]
    private static partial Regex QuantifiedRegex();
}
