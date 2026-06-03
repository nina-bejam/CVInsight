using AIResumeAnalyzer.Domain.Models;

namespace AIResumeAnalyzer.Infrastructure.Analysis;

internal static class HumanFeedbackBuilder
{
    public static IReadOnlyList<string> BuildFeedback(
        ResumeProfile profile,
        string resumeText,
        string targetRole,
        IReadOnlyList<string> detected,
        IReadOnlyList<string> missing)
    {
        var feedback = new List<string>();
        var lower = resumeText.ToLowerInvariant();
        var roleLower = targetRole.ToLowerInvariant();

        AddSkillVisibilityFeedback(feedback, profile, detected);
        AddRoleAlignmentFeedback(feedback, profile, roleLower, detected, missing, lower);
        AddStructureFeedback(feedback, profile);
        AddContentDepthFeedback(feedback, profile, lower);

        if (feedback.Count == 0)
            feedback.Add("The CV reads clearly — tighten a few bullets with numbers so recruiters can scan impact faster.");

        return feedback.Take(7).ToList();
    }

    public static IReadOnlyList<string> BuildImprovements(
        ResumeProfile profile,
        string targetRole,
        IReadOnlyList<string> detected,
        IReadOnlyList<string> missing)
    {
        var suggestions = new List<string>();

        foreach (var skill in missing.Take(3))
            suggestions.Add($"If you have {skill} experience, mention it in a recent role or project — it's commonly expected for {targetRole} roles.");

        if (!profile.HasProjectsSection && missing.Count > 0)
            suggestions.Add("A short Projects section would help you show hands-on work with tools you haven't listed in a job title yet.");

        if (profile.QuantifiedBulletCount < 2)
            suggestions.Add("Pick two or three bullets and add numbers (team size, performance gain, revenue, or time saved).");

        if (profile.BulletPointCount < 5)
            suggestions.Add("Break long paragraphs into bullets so hiring managers can skim your responsibilities quickly.");

        if (detected.Count >= 6 && missing.Count <= 2)
            suggestions.Add("You're close on skills — mirror the exact wording from job posts for your target role in the top third of the CV.");

        return suggestions.Distinct().Take(6).ToList();
    }

    public static string BuildSummary(
        ResumeProfile profile,
        string targetRole,
        IReadOnlyList<string> detected)
    {
        var name = profile.FullName;
        var title = profile.CurrentTitle ?? targetRole;
        var topSkills = detected.Take(4).ToList();

        if (profile.YearsOfExperience > 0 && topSkills.Count > 0)
            return $"{name} looks like a {title} with about {profile.YearsOfExperience} years of experience, leaning on {string.Join(", ", topSkills)}.";

        if (topSkills.Count > 0)
            return $"{name} is positioning for {targetRole} work, with {string.Join(", ", topSkills)} showing up in the CV.";

        return $"{name} is aiming at {targetRole} roles — the CV would benefit from naming specific tools and frameworks used in recent work.";
    }

    public static string BuildLinkedInHeadline(
        ResumeProfile profile,
        string targetRole,
        IReadOnlyList<string> detected)
    {
        var highlight = detected.FirstOrDefault() ?? profile.CurrentTitle ?? targetRole;
        var years = profile.YearsOfExperience;

        return years > 0
            ? $"{targetRole} · {highlight} · {years}+ years"
            : $"{targetRole} · {highlight} · Open to new opportunities";
    }

    private static void AddSkillVisibilityFeedback(
        List<string> feedback,
        ResumeProfile profile,
        IReadOnlyList<string> detected)
    {
        foreach (var skill in detected)
        {
            if (profile.SkillMentionCounts.TryGetValue(skill, out var count) && count >= 2)
            {
                feedback.Add($"Your {skill} experience is visible throughout the CV.");
                break;
            }
        }

        if (detected.Contains("Angular", StringComparer.OrdinalIgnoreCase) &&
            profile.SkillMentionCounts.GetValueOrDefault("Angular") >= 1)
            feedback.Add("Your Angular experience is visible throughout the CV.");

        if (detected.Contains("React", StringComparer.OrdinalIgnoreCase) &&
            !feedback.Any(f => f.Contains("React", StringComparison.OrdinalIgnoreCase)))
            feedback.Add("React shows up in your CV — make sure your strongest project using it sits near the top of Experience.");
    }

    private static void AddRoleAlignmentFeedback(
        List<string> feedback,
        ResumeProfile profile,
        string roleLower,
        IReadOnlyList<string> detected,
        IReadOnlyList<string> missing,
        string resumeLower)
    {
        var hasFrontend = detected.Any(s => s is "Angular" or "React" or "Vue" or "HTML" or "CSS");
        var hasBackend = detected.Any(s => s is "C#" or ".NET" or "ASP.NET Core" or "Java" or "Python" or "SQL" or "REST API");

        if ((roleLower.Contains("full") || roleLower.Contains("stack")) && hasFrontend && !hasBackend)
            feedback.Add("I would add more information about the backend projects you worked on.");

        if (roleLower.Contains("back") && hasFrontend && !hasBackend)
            feedback.Add("The CV reads frontend-heavy — call out APIs, databases, and services you owned for backend-focused roles.");

        if (roleLower.Contains("front") && hasBackend && !hasFrontend)
            feedback.Add("Backend tools dominate the CV — surface UI work, component libraries, or design collaboration for frontend roles.");

        if (missing.Count > 0)
        {
            var topMissing = string.Join(", ", missing.Take(3));
            feedback.Add($"For {roleLower} roles, I'd expect to see more on: {topMissing}.");
        }

        if (resumeLower.Contains("skills") && detected.Count >= 4 && profile.HasSkillsSection)
            feedback.Add("The skills section is clear, but the project descriptions could be more detailed.");
    }

    private static void AddStructureFeedback(List<string> feedback, ResumeProfile profile)
    {
        if (!profile.HasSummarySection)
            feedback.Add("There's no short summary at the top — a three-line intro helps recruiters place you quickly.");

        if (!profile.HasSkillsSection && profile.SkillMentionCounts.Count >= 3)
            feedback.Add("Tools appear in your experience, but a dedicated Skills section would make scanning easier.");

        if (!profile.HasExperienceSection)
            feedback.Add("I couldn't spot a clear Experience section — grouping roles under one heading will help readability.");

        if (profile.Email is null)
            feedback.Add("Add an email address near your name so recruiters can reach you without leaving LinkedIn.");
    }

    private static void AddContentDepthFeedback(List<string> feedback, ResumeProfile profile, string resumeLower)
    {
        if (profile.BulletPointCount >= 3 && profile.QuantifiedBulletCount == 0)
            feedback.Add("Your bullets describe responsibilities well — adding metrics would make outcomes easier to compare.");

        if (profile.HasProjectsSection && profile.BulletPointCount < 4)
            feedback.Add("You list projects, but each one could use a line on your role, stack, and what shipped.");

        if (resumeLower.Length < 500)
            feedback.Add("The CV is quite short — expand your two most recent roles with context, stack, and results.");
    }
}
