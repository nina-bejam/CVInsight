using AIResumeAnalyzer.Application.DTOs;

namespace AIResumeAnalyzer.Application.Interfaces;

public interface IResumeAnalyzerService
{
    Task<ResumeAnalysisResponseDto> AnalyzeAsync(
        AnalyzeResumeRequestDto request,
        CancellationToken cancellationToken = default);
}
