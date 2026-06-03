namespace AIResumeAnalyzer.Infrastructure.Analysis;

internal static class SkillMatcher
{
    public static IReadOnlyList<string> DetectSkills(string resumeText)
    {
        var lower = resumeText.ToLowerInvariant();
        return SkillDictionary.TechnicalSkills
            .Where(entry => entry.Keywords.Any(kw => ContainsKeyword(lower, kw)))
            .Select(entry => entry.Skill)
            .OrderBy(s => s)
            .ToList();
    }

    public static IReadOnlyList<string> FindMissingSkills(
        IReadOnlyList<string> detected,
        string targetRole,
        int maxResults = 8)
    {
        var required = SkillDictionary.ResolveRoleSkills(targetRole);
        return required
            .Where(skill => !detected.Contains(skill, StringComparer.OrdinalIgnoreCase))
            .Take(maxResults)
            .ToList();
    }

    private static bool ContainsKeyword(string lowerText, string keyword)
    {
        var index = lowerText.IndexOf(keyword, StringComparison.Ordinal);
        if (index < 0) return false;

        if (keyword.Length > 3)
            return true;

        return IsWordBoundary(lowerText, index, keyword.Length);
    }

    private static bool IsWordBoundary(string text, int index, int length)
    {
        var beforeOk = index == 0 || !char.IsLetterOrDigit(text[index - 1]);
        var afterIndex = index + length;
        var afterOk = afterIndex >= text.Length || !char.IsLetterOrDigit(text[afterIndex]);
        return beforeOk && afterOk;
    }
}
