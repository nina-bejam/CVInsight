using AIResumeAnalyzer.Application.Interfaces;
using AIResumeAnalyzer.Domain.Models;

namespace AIResumeAnalyzer.Infrastructure.Analysis;

public sealed class RuleBasedResumeAnalysisEngine : IResumeAnalysisEngine
{
    public ResumeAnalysis Analyze(string resumeText, string targetRole)
    {
        var profile = ResumeTextParser.Parse(resumeText);
        var detected = SkillMatcher.DetectSkills(resumeText);
        var missing = SkillMatcher.FindMissingSkills(detected, targetRole);

        return new ResumeAnalysis
        {
            Summary = HumanFeedbackBuilder.BuildSummary(profile, targetRole, detected),
            DetectedSkills = detected,
            MissingSkills = missing,
            Feedback = HumanFeedbackBuilder.BuildFeedback(profile, resumeText, targetRole, detected, missing),
            LinkedInHeadline = HumanFeedbackBuilder.BuildLinkedInHeadline(profile, targetRole, detected),
            ImprovementSuggestions = HumanFeedbackBuilder.BuildImprovements(profile, targetRole, detected, missing)
        };
    }
}
