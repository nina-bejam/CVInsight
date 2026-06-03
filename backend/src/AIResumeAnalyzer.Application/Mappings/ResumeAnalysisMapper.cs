using AIResumeAnalyzer.Application.DTOs;
using AIResumeAnalyzer.Domain.Models;

namespace AIResumeAnalyzer.Application.Mappings;

public static class ResumeAnalysisMapper
{
    public static ResumeAnalysisResponseDto ToDto(ResumeAnalysis analysis) =>
        new()
        {
            Summary = analysis.Summary,
            DetectedSkills = analysis.DetectedSkills,
            MissingSkills = analysis.MissingSkills,
            Feedback = analysis.Feedback,
            LinkedInHeadline = analysis.LinkedInHeadline,
            ImprovementSuggestions = analysis.ImprovementSuggestions
        };
}
