using AIResumeAnalyzer.Application.DTOs;
using AIResumeAnalyzer.Application.Interfaces;
using AIResumeAnalyzer.Application.Mappings;

namespace AIResumeAnalyzer.Application.Services;

public sealed class ResumeAnalyzerService(IResumeAnalysisEngine engine) : IResumeAnalyzerService
{
    public Task<ResumeAnalysisResponseDto> AnalyzeAsync(
        AnalyzeResumeRequestDto request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var resumeText = request.ResumeText.Trim();
        var targetRole = request.TargetRole.Trim();

        var analysis = engine.Analyze(resumeText, targetRole);
        return Task.FromResult(ResumeAnalysisMapper.ToDto(analysis));
    }
}
